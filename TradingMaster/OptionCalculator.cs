using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingMaster.CodeSet;
using TradingMaster.JYData;
using System.Threading;
using System.Threading.Tasks;

namespace TradingMaster
{
    public class OptionCalculator
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static SynQueue<object>  _RealDataQueue = new SynQueue<object>();

        public static void Enqueue(object data)
        {
            if (data != null)
            {
                _RealDataQueue.Enqueue(data, false);
            }
        }

        private static Task CalculatorTask = Task.Factory.StartNew(() => CalculatorThreadProc(cts.Token), cts.Token);

        private static void CalculatorThreadProc(CancellationToken ct)
        {
            List<OptionRealData> optionMdList = new List<OptionRealData>();
            while ((!ct.IsCancellationRequested))//(ExecFlag)
            {
                List<object> dataList = _RealDataQueue.DequeueAll();
                foreach (object data in dataList)
                {
                    if (data is OptionRealData)
                    {
                        optionMdList.Add((OptionRealData)data);
                    }
                    else
                    {
                        Util.Log("RealDataQueue: Error for Option Real Data!");
                    }
                }
                if (optionMdList.Count > 0)
                {
                    CalculateFromOptionRealData(optionMdList);
                    optionMdList.Clear();
                }
            }
        }

        private static void CalculateFromOptionRealData(List<OptionRealData> optionRealDataLst)
        {
            try
            {
                foreach (OptionRealData optRealData in optionRealDataLst)
                {
                    UpdateGreekLetters(optRealData);
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        private static void UpdateGreekLetters(OptionRealData optionData)
        {
            Contract callOpt = CodeSetManager.GetContractInfo(optionData.Code_C, CodeSetManager.ExNameToCtp(optionData.Market));
            Contract putOpt = CodeSetManager.GetContractInfo(optionData.Code_P, CodeSetManager.ExNameToCtp(optionData.Market));
            Contract fCode = null;

            double r = 0.000025;
            double q = 0;
            if (callOpt.ProductType == "SpotOption")
            {
                q = 0; // ToDo: Dividend
            }
            else // Futures Option
            {
                q = r;
            }

            if (callOpt != null && putOpt != null)
            {
                fCode = CodeSetManager.GetContractInfo(CodeSetManager.GetOptionUnderlyingCode(callOpt, optionData.ExchCode), optionData.ExchCode);
                if (fCode != null)
                {
                    RealData futRealData = DataContainer.GetRealDataFromContainer(fCode);
                    RealData callRealData = DataContainer.GetRealDataFromContainer(callOpt);
                    RealData putRealData = DataContainer.GetRealDataFromContainer(putOpt);
                    if (futRealData != null)
                    {
                        if (callRealData != null)
                        {
                            optionData.Sigma_C = GetImpliedVolatility(callOpt, callRealData.MidBasePrice, futRealData.MidBasePrice, 0, 0.000025, true);
                            if (!double.IsInfinity(optionData.Sigma_C) && !double.IsNaN(optionData.Sigma_C) && !double.IsInfinity(optionData.Sigma_C))
                            {
                                optionData.Delta_C = GetInstantDelta(callOpt, callRealData.MidBasePrice, futRealData.MidBasePrice, optionData.Sigma_C, q, r);
                                optionData.Gamma_C = GetInstantGamma(callOpt, callRealData.MidBasePrice, futRealData.MidBasePrice, optionData.Sigma_C, q, r);
                                optionData.Vega_C = GetInstantVega(callOpt, callRealData.MidBasePrice, futRealData.MidBasePrice, optionData.Sigma_C, q, r);
                                optionData.Theta_C = GetInstantTheta(callOpt, callRealData.MidBasePrice, futRealData.MidBasePrice, optionData.Sigma_C, q, r);
                            }
                        }
                        if (putRealData != null)
                        {
                            optionData.Sigma_P = GetImpliedVolatility(putOpt, putRealData.NewPrice, futRealData.NewPrice, 0, 0.000025, true);
                            if (!double.IsInfinity(optionData.Sigma_P) && !double.IsNaN(optionData.Sigma_P) && !double.IsInfinity(optionData.Sigma_P))
                            {
                                optionData.Delta_P = GetInstantDelta(putOpt, putRealData.MidBasePrice, futRealData.MidBasePrice, optionData.Sigma_P, q, r);
                                optionData.Gamma_P = GetInstantGamma(putOpt, putRealData.MidBasePrice, futRealData.MidBasePrice, optionData.Sigma_P, q, r);
                                optionData.Vega_P = GetInstantVega(putOpt, putRealData.MidBasePrice, futRealData.MidBasePrice, optionData.Sigma_P, q, r);
                                optionData.Theta_P = GetInstantTheta(putOpt, putRealData.MidBasePrice, futRealData.MidBasePrice, optionData.Sigma_P, q, r);
                            }
                        }
                    }
                    else
                    {
                        Util.Log(String.Format("Warning! illegal Contract Quote Data: fCode = {0}", fCode.Code));
                    }
                }
                else
                {
                    Util.Log("Warning! illegal Futures Contract! Code: " + optionData.Code_C + " " + optionData.Code_P);
                }
            }
            else
            {
                Util.Log("Warning! illegal Option Contract!");
            }
        }

        private static double[] NORMSCDF = {0.5000, 0.5040, 0.5080, 0.5120, 0.5160, 0.5199, 0.5239, 0.5279, 0.5319, 0.5359,
                                            0.5398, 0.5438, 0.5478, 0.5517, 0.5557, 0.5596, 0.5636, 0.5675, 0.5714, 0.5753,
                                            0.5793, 0.5832, 0.5871, 0.5910, 0.5948, 0.5987, 0.6026, 0.6064, 0.6103, 0.6141,
                                            0.6179, 0.6217, 0.6255, 0.6293, 0.6331, 0.6368, 0.6406, 0.6443, 0.6480, 0.6517,
                                            0.6554, 0.6591, 0.6628, 0.6664, 0.6700, 0.6736, 0.6772, 0.6808, 0.6844, 0.6879,
                                            0.6915, 0.6950, 0.6985, 0.7019, 0.7054, 0.7088, 0.7123, 0.7157, 0.7190, 0.7224,
                                            0.7257, 0.7291, 0.7324, 0.7357, 0.7389, 0.7422, 0.7454, 0.7486, 0.7517, 0.7549,
                                            0.7580, 0.7611, 0.7642, 0.7673, 0.7704, 0.7734, 0.7764, 0.7794, 0.7823, 0.7852,
                                            0.7881, 0.7910, 0.7939, 0.7967, 0.7995, 0.8023, 0.8051, 0.8078, 0.8106, 0.8133,
                                            0.8159, 0.8186, 0.8212, 0.8238, 0.8264, 0.8289, 0.8315, 0.8340, 0.8365, 0.8389,
                                            0.8413, 0.8438, 0.8461, 0.8485, 0.8508, 0.8531, 0.8554, 0.8577, 0.8599, 0.8621,
                                            0.8643, 0.8665, 0.8686, 0.8708, 0.8729, 0.8749, 0.8770, 0.8790, 0.8810, 0.8830,
                                            0.8849, 0.8869, 0.8888, 0.8907, 0.8925, 0.8944, 0.8962, 0.8980, 0.8997, 0.9015,
                                            0.9032, 0.9049, 0.9066, 0.9082, 0.9099, 0.9115, 0.9131, 0.9147, 0.9162, 0.9177,
                                            0.9192, 0.9207, 0.9222, 0.9236, 0.9251, 0.9265, 0.9279, 0.9292, 0.9306, 0.9319,
                                            0.9332, 0.9345, 0.9357, 0.9370, 0.9382, 0.9394, 0.9406, 0.9418, 0.9429, 0.9441,
                                            0.9452, 0.9463, 0.9474, 0.9484, 0.9495, 0.9505, 0.9515, 0.9525, 0.9535, 0.9545,
                                            0.9554, 0.9564, 0.9573, 0.9582, 0.9591, 0.9599, 0.9608, 0.9616, 0.9625, 0.9633,
                                            0.9641, 0.9649, 0.9656, 0.9664, 0.9671, 0.9678, 0.9686, 0.9693, 0.9699, 0.9706,
                                            0.9713, 0.9719, 0.9726, 0.9732, 0.9738, 0.9744, 0.9750, 0.9756, 0.9761, 0.9767,
                                            0.9772, 0.9778, 0.9783, 0.9788, 0.9793, 0.9798, 0.9803, 0.9808, 0.9812, 0.9817,
                                            0.9821, 0.9826, 0.9830, 0.9834, 0.9838, 0.9842, 0.9846, 0.9850, 0.9854, 0.9857,
                                            0.9861, 0.9864, 0.9868, 0.9871, 0.9875, 0.9878, 0.9881, 0.9884, 0.9887, 0.9890,
                                            0.9893, 0.9896, 0.9898, 0.9901, 0.9904, 0.9906, 0.9909, 0.9911, 0.9913, 0.9916,
                                            0.9918, 0.9920, 0.9922, 0.9925, 0.9927, 0.9929, 0.9931, 0.9932, 0.9934, 0.9936,
                                            0.9938, 0.9940, 0.9941, 0.9943, 0.9945, 0.9946, 0.9948, 0.9949, 0.9951, 0.9952,
                                            0.9953, 0.9955, 0.9956, 0.9957, 0.9959, 0.9960, 0.9961, 0.9962, 0.9963, 0.9964,
                                            0.9965, 0.9966, 0.9967, 0.9968, 0.9969, 0.9970, 0.9971, 0.9972, 0.9973, 0.9974,
                                            0.9974, 0.9975, 0.9976, 0.9977, 0.9977, 0.9978, 0.9979, 0.9979, 0.9980, 0.9981,
                                            0.9981, 0.9982, 0.9982, 0.9983, 0.9984, 0.9984, 0.9985, 0.9985, 0.9986, 0.9986,
                                            0.9987, 0.9987, 0.9987, 0.9988, 0.9988, 0.9989, 0.9989, 0.9989, 0.9990, 0.9990,
                                            0.9991, 0.9991, 0.9991, 0.9991, 0.9992, 0.9992, 0.9992, 0.9992, 0.9993, 0.9993,
                                            0.9993, 0.9993, 0.9994, 0.9994, 0.9994, 0.9994, 0.9994, 0.9995, 0.9995, 0.9995,
                                            0.9995, 0.9995, 0.9995, 0.9996, 0.9996, 0.9996, 0.9996, 0.9996, 0.9996, 0.9997,
                                            0.9997, 0.9997, 0.9997, 0.9997, 0.9997, 0.9997, 0.9997, 0.9997, 0.9997, 0.9998,
                                            0.9998, 0.9998, 0.9998, 0.9998, 0.9998, 0.9998, 0.9998, 0.9998, 0.9998, 0.9998,
                                            0.9998, 0.9998, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999,
                                            0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999,
                                            0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999, 0.9999,
                                            1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000,
                                            1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000, 1.0000};

        private static double NormStdDist(double d)
        {
            try
            {
                double sign = 1;

                if (double.IsNaN(d))
                {
                    return 0;
                }

                if (d < 0)
                {
                    sign = -1;
                    d = -d;
                }
                if (d > 4.0) d = 4.0;

                double pre = Math.Floor(d * 100) / 100.0;
                double post = Math.Min(4.0, pre + 0.01);

                double ratio = (d - pre) * 100;

                pre = NORMSCDF[Math.Min((int)(pre * 100), NORMSCDF.Length - 1)];
                post = NORMSCDF[Math.Min((int)(post * 100), NORMSCDF.Length - 1)];

                d = pre * (1 - ratio) + post * ratio;

                if (sign == 1)
                    return d;
                else
                    return 1 - d;
            }
            catch (Exception ex)
            {
                Util.Log(ex.Message);
                Util.Log(ex.StackTrace);
                return 0;
            }
        }

        // 根据B-S公式计算报价_European Call
        public static double EuCallOpt_BsFormula(double s, double k, double T, double sigma, double q, double r)
        {
            double bsValue = 0;
            double d1 = (Math.Log(s / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
            double d2 = d1 - sigma * Math.Sqrt(T);

            bsValue = s * Math.Exp(-q * T) * NormStdDist(d1) - k * Math.Exp(-r * T) * NormStdDist(d2);

            return bsValue;
        }

        // 根据B-S公式计算报价_European Put
        public static double EuPutOpt_BsFormula(double s, double k, double T, double sigma, double q, double r)
        {
            double bsValue = 0;
            double d1 = (Math.Log(s / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
            double d2 = d1 - sigma * Math.Sqrt(T);

            bsValue = -s * Math.Exp(-q * T) * NormStdDist(-d1) + k * Math.Exp(-r * T) * NormStdDist(-d2);

            return bsValue;
        }

        // 计算隐含波动率_European Call
        public static double EuCallOpt_ImpVolatility(double bsOptVal, double s0, double k, double T, double q, double r)
        {
            double sigma = 0.05;
            double calError = 10;
            while (Math.Abs(calError) > 1 && !double.IsInfinity(sigma))
            {
                double d1 = (Math.Log(s0 / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
                double d2 = d1 - sigma * Math.Sqrt(T);
                double diffNd1 = Math.Exp(-d2 * d2 / 2 - sigma * d2 * Math.Sqrt(T) - sigma * sigma * T / 2) / Math.Sqrt(2 * Math.PI);
                double diffNd2 = s0 * diffNd1 / (k * Math.Exp(-r * T));
                Util.Log(String.Format("sigma = {0}, calError = {1}, step = {2}", sigma, calError, (EuCallOpt_BsFormula(s0, k, T, sigma, q, r) - bsOptVal) / (s0 * Math.Sqrt(T) * diffNd1 * Math.Exp(-q * T)) ));
                sigma = sigma - ((EuCallOpt_BsFormula(s0, k, T, sigma, q, r) - bsOptVal) / (s0 * Math.Sqrt(T) * diffNd1 * Math.Exp(-q * T)));
                    //((s * Math.Exp(-q * T) * diffNd1 + k * Math.Exp(-r * T) * diffNd2) * Math.Sqrt(T) + (k * Math.Exp(-r * T) * d2 * diffNd2 - s * Math.Exp(-q * T) * d1 * diffNd1) / sigma));
                calError = EuCallOpt_BsFormula(s0, k, T, sigma, q, r) - bsOptVal;
            }
            if (double.IsInfinity(sigma) || double.IsNaN(sigma))
            {
                Util.Log("Waringing! Illegal implied Volatility!");
                sigma = double.NegativeInfinity;
            }
            return sigma;
        }

        // 计算隐含波动率_European Put
        public static double EuPutOpt_ImpVolatility(double bsOptVal, double s0, double k, double T, double q, double r)
        {
            double sigma = 0.05;
            double calError = 10;
            while (Math.Abs(calError) > 1 && !double.IsInfinity(sigma))
            {
                double d1 = (Math.Log(s0 / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
                double d2 = d1 - sigma * Math.Sqrt(T);
                double diffNd1 = Math.Exp(-d2 * d2 / 2 - sigma * d2 * Math.Sqrt(T) - sigma * sigma * T / 2) / Math.Sqrt(2 * Math.PI);
                double diffNd2 = s0 * diffNd1 / (k * Math.Exp(-r * T));
                Util.Log(String.Format("sigma = {0}, calError = {1}, step = {2}", sigma, calError, (EuPutOpt_BsFormula(s0, k, T, sigma, q, r) - bsOptVal) / (s0 * Math.Sqrt(T) * diffNd1 * Math.Exp(-q * T)) ));
                sigma = sigma - ((EuPutOpt_BsFormula(s0, k, T, sigma, q, r) - bsOptVal) / (s0 * Math.Sqrt(T) * diffNd1 * Math.Exp(-q * T)));
                    //((-s * Math.Exp(-q * T) * diffNd1 - k * Math.Exp(-r * T) * diffNd2) * Math.Sqrt(T) - (k * Math.Exp(-r * T) * d2 * diffNd2 - s * Math.Exp(-q * T) * d1 * diffNd1) / sigma));
                calError = EuPutOpt_BsFormula(s0, k, T, sigma, q, r) - bsOptVal;
            }
            if (double.IsInfinity(sigma) || double.IsNaN(sigma))
            {
                Util.Log("Waringing! Illegal implied Volatility!");
                sigma = double.NegativeInfinity;
            }
            return sigma;
        }

        // 根据报价算隐含波动率_European Call
        public static double EuCallOpt_ImpVolatility_BiTree(double opts, double s, double exep, double T, double q, double r, double pankou)
        {
            double y = 0;

            double tpvol = 0.02;
            double lastvol = 0.012;
            double lastopts = EuCallOpt_BsFormula(s, exep, T, lastvol, q, r);

            for (int ci = 0; ci < 200; ci++)
            {
                double tpopts = EuCallOpt_BsFormula(s, exep, T, tpvol, q, r);
                if (Math.Abs(tpopts - opts) < pankou)
                {
                    y = tpvol;
                    return y;
                }
                else
                {
                    if (opts > tpopts)
                    {
                        if (opts < lastopts)
                        {
                            y = tpvol;
                            tpvol = (lastvol + tpvol) / 2;
                            lastvol = y;
                            lastopts = tpopts;
                        }
                        else
                        {
                            y = tpvol;
                            tpvol = 2 * tpvol;
                            lastvol = y;
                            lastopts = tpopts;
                        }
                    }
                    else
                    {
                        if (opts > lastopts)
                        {
                            y = tpvol;
                            tpvol = (lastvol + tpvol) / 2;
                            lastvol = y;
                            lastopts = tpopts;
                        }
                        else
                        {
                            y = tpvol;
                            tpvol = tpvol / 2;
                            lastvol = y;
                            lastopts = tpopts;
                        }
                    }
                }
            }

            return y;
        }

        // 根据报价算隐含波动率_European Put
        public static double EuPutOpt_ImpVolatility_BiTree(double opts, double s, double exep, double T, double q, double r, double pankou)
        {
            double y = 0;

            double tpvol = 0.02;
            double lastvol = 0.012;
            double lastopts = EuPutOpt_BsFormula(s, exep, T, lastvol, q, r);

            for (int ci = 0; ci < 200; ci++)
            {
                double tpopts = EuPutOpt_BsFormula(s, exep, T, tpvol, q, r);
                if (Math.Abs(tpopts - opts) < pankou)
                {
                    y = tpvol;
                    return y;
                }
                else
                {
                    if (opts > tpopts)
                    {
                        if (opts < lastopts)
                        {
                            y = tpvol;
                            tpvol = (lastvol + tpvol) / 2;
                            lastvol = y;
                            lastopts = tpopts;
                        }
                        else
                        {
                            y = tpvol;
                            tpvol = 2 * tpvol;
                            lastvol = y;
                            lastopts = tpopts;
                        }
                    }
                    else
                    {
                        if (opts > lastopts)
                        {
                            y = tpvol;
                            tpvol = (lastvol + tpvol) / 2;
                            lastvol = y;
                            lastopts = tpopts;
                        }
                        else
                        {
                            y = tpvol;
                            tpvol = tpvol / 2;
                            lastvol = y;
                            lastopts = tpopts;
                        }
                    }
                }
            }

            return y;
        }

        // 根据二叉树计算报价_European Call
        public static double EuCallOpt_BiTree(double s, double k, double T, double sigma, double q, double r)
        {
            double y = 0;

            int stepnum = 108;//128;
            if (T < 70)
            {
                stepnum = 72;
            }
            else if (T >= 70 && T <= 130)
            {
                stepnum = 96;
            }
            else if (T > 130)
            {
                stepnum = 108;
            }

            if (T == 0)
                y = 0;
            else
            {
                double timeinterval = T / (stepnum - 1);
                double R = (r - q) * timeinterval;

                List<List<double>> optionmat = new List<List<double>>();
                List<List<double>> valuemat = new List<List<double>>();
                for (int i = 0; i < stepnum; i++)
                {
                    optionmat.Add(new List<double>());
                    for (int j = 0; j < stepnum; j++)
                        optionmat[i].Add(0);
                }
                for (int i = 0; i < stepnum; i++)
                {
                    valuemat.Add(new List<double>());
                    for (int j = 0; j < stepnum; j++)
                        valuemat[i].Add(0);
                }

                double u = Math.Exp(sigma * Math.Sqrt(timeinterval));
                double d = Math.Exp(-sigma * Math.Sqrt(timeinterval));
                double p = (Math.Exp(R) - d) / (u - d);

                valuemat[0][0] = s;

                for (int ci = 1; ci < stepnum; ci++)
                {
                    for (int cj = 0; cj < ci; cj++)
                    {
                        valuemat[cj][ci] = valuemat[cj][ci - 1] * u;
                    }
                    valuemat[ci][ci] = valuemat[ci - 1][ci - 1] * d;
                }

                for (int ci = 0; ci < stepnum; ci++)
                {
                    if (valuemat[ci][stepnum - 1] > k)
                        optionmat[ci][stepnum - 1] = valuemat[ci][stepnum - 1] - k;
                    else
                        optionmat[ci][stepnum - 1] = 0;
                }

                for (int ci = stepnum - 2; ci >= 0; ci--)
                    for (int cj = 0; cj <= ci; cj++)
                    {
                        optionmat[cj][ci] = (optionmat[cj][ci + 1] * p + optionmat[cj + 1][ci + 1] * (1 - p)) / Math.Exp(R);

                        //if ((valuemat[cj][ci] - k) > optionmat[cj][ci])
                        //    optionmat[cj][ci] = valuemat[cj][ci] - k;
                    }

                y = optionmat[0][0];

            }

            return y;

        }

        // 根据二叉树计算报价_European Put
        public static double EuPutOpt_BiTree(double s, double k, double T, double sigma, double q, double r)
        {
            double y = 0;

            int stepnum = 108;//128;
            if (T < 70)
            {
                stepnum = 72;
            }
            else if (T >= 70 && T <= 130)
            {
                stepnum = 96;
            }
            else if (T > 130)
            {
                stepnum = 108;
            }

            if (T == 0)
                y = 0;
            else
            {
                double timeinterval = T / (stepnum - 1);
                double R = (r - q) * timeinterval;

                List<List<double>> optionmat = new List<List<double>>();
                List<List<double>> valuemat = new List<List<double>>();
                for (int i = 0; i < stepnum; i++)
                {
                    optionmat.Add(new List<double>());
                    for (int j = 0; j < stepnum; j++)
                        optionmat[i].Add(0);
                }
                for (int i = 0; i < stepnum; i++)
                {
                    valuemat.Add(new List<double>());
                    for (int j = 0; j < stepnum; j++)
                        valuemat[i].Add(0);
                }

                double u = Math.Exp(sigma * Math.Sqrt(timeinterval));
                double d = Math.Exp(-sigma * Math.Sqrt(timeinterval));
                double p = (Math.Exp(R) - d) / (u - d);

                valuemat[0][0] = s;

                for (int ci = 1; ci < stepnum; ci++)
                {
                    for (int cj = 0; cj < ci; cj++)
                    {
                        valuemat[cj][ci] = valuemat[cj][ci - 1] * u;
                    }
                    valuemat[ci][ci] = valuemat[ci - 1][ci - 1] * d;
                }

                for (int ci = 0; ci < stepnum; ci++)
                {
                    if (valuemat[ci][stepnum - 1] < k)
                        optionmat[ci][stepnum - 1] = k - valuemat[ci][stepnum - 1];
                    else
                        optionmat[ci][stepnum - 1] = 0;
                }

                for (int ci = stepnum - 2; ci >= 0; ci--)
                    for (int cj = 0; cj <= ci; cj++)
                    {
                        optionmat[cj][ci] = (optionmat[cj][ci + 1] * p + optionmat[cj + 1][ci + 1] * (1 - p)) / Math.Exp(R);

                        //if ((k - valuemat[cj][ci]) > optionmat[cj][ci])
                        //    optionmat[cj][ci] = k - valuemat[cj][ci];
                    }

                y = optionmat[0][0];

            }

            return y;

        }

        // 根据二叉树计算报价_American Call
        public static double UsCallOpt_BiTree(double s, double k, double T, double sigma, double q, double r)
        {
            double y = 0;

            int stepnum = 108;//128;
            if (T < 70)
            {
                stepnum = 72;
            }
            else if (T >= 70 && T <= 130)
            {
                stepnum = 96;
            }
            else if (T > 130)
            {
                stepnum = 108;
            }

            if (T == 0)
                y = 0;
            else
            {
                double timeinterval = T / (stepnum - 1);
                double R = (r - q) * timeinterval;

                List<List<double>> optionmat = new List<List<double>>();
                List<List<double>> valuemat = new List<List<double>>();
                for (int i = 0; i < stepnum; i++)
                {
                    optionmat.Add(new List<double>());
                    for (int j = 0; j < stepnum; j++)
                        optionmat[i].Add(0);
                }
                for (int i = 0; i < stepnum; i++)
                {
                    valuemat.Add(new List<double>());
                    for (int j = 0; j < stepnum; j++)
                        valuemat[i].Add(0);
                }

                double u = Math.Exp(sigma * Math.Sqrt(timeinterval));
                double d = Math.Exp(-sigma * Math.Sqrt(timeinterval));
                double p = (Math.Exp(R) - d) / (u - d);

                valuemat[0][0] = s;

                for (int ci = 1; ci < stepnum; ci++)
                {
                    for (int cj = 0; cj < ci; cj++)
                    {
                        valuemat[cj][ci] = valuemat[cj][ci - 1] * u;
                    }
                    valuemat[ci][ci] = valuemat[ci - 1][ci - 1] * d;
                }

                for (int ci = 0; ci < stepnum; ci++)
                {
                    if (valuemat[ci][stepnum - 1] > k)
                        optionmat[ci][stepnum - 1] = valuemat[ci][stepnum - 1] - k;
                    else
                        optionmat[ci][stepnum - 1] = 0;
                }

                for (int ci = stepnum - 2; ci >= 0; ci--)
                    for (int cj = 0; cj <= ci; cj++)
                    {
                        optionmat[cj][ci] = (optionmat[cj][ci + 1] * p + optionmat[cj + 1][ci + 1] * (1 - p)) / Math.Exp(R);

                        if ((valuemat[cj][ci] - k) > optionmat[cj][ci])
                            optionmat[cj][ci] = valuemat[cj][ci] - k;
                    }

                y = optionmat[0][0];

            }

            return y;

        }

        // 根据二叉树计算报价_American Put
        public static double UsPutOpt_BiTree(double s, double k, double T, double sigma, double q, double r)
        {
            double y = 0;

            int stepnum = 108;//128;
            if (T < 70)
            {
                stepnum = 72;
            }
            else if (T >= 70 && T <= 130)
            {
                stepnum = 96;
            }
            else if (T > 130)
            {
                stepnum = 108;
            }

            if (T == 0)
                y = 0;
            else
            {
                double timeinterval = T / (stepnum - 1);
                double R = (r - q) * timeinterval;

                List<List<double>> optionmat = new List<List<double>>();
                List<List<double>> valuemat = new List<List<double>>();
                for (int i = 0; i < stepnum; i++)
                {
                    optionmat.Add(new List<double>());
                    for (int j = 0; j < stepnum; j++)
                        optionmat[i].Add(0);
                }
                for (int i = 0; i < stepnum; i++)
                {
                    valuemat.Add(new List<double>());
                    for (int j = 0; j < stepnum; j++)
                        valuemat[i].Add(0);
                }

                double u = Math.Exp(sigma * Math.Sqrt(timeinterval));
                double d = Math.Exp(-sigma * Math.Sqrt(timeinterval));
                double p = (Math.Exp(R) - d) / (u - d);

                valuemat[0][0] = s;

                for (int ci = 1; ci < stepnum; ci++)
                {
                    for (int cj = 0; cj < ci; cj++)
                    {
                        valuemat[cj][ci] = valuemat[cj][ci - 1] * u;
                    }
                    valuemat[ci][ci] = valuemat[ci - 1][ci - 1] * d;
                }

                for (int ci = 0; ci < stepnum; ci++)
                {
                    if (valuemat[ci][stepnum - 1] < k)
                        optionmat[ci][stepnum - 1] = k - valuemat[ci][stepnum - 1];
                    else
                        optionmat[ci][stepnum - 1] = 0;
                }

                for (int ci = stepnum - 2; ci >= 0; ci--)
                    for (int cj = 0; cj <= ci; cj++)
                    {
                        optionmat[cj][ci] = (optionmat[cj][ci + 1] * p + optionmat[cj + 1][ci + 1] * (1 - p)) / Math.Exp(R);

                        if ((k - valuemat[cj][ci]) > optionmat[cj][ci])
                            optionmat[cj][ci] = k - valuemat[cj][ci];
                    }

                y = optionmat[0][0];

            }

            return y;

        }

        // 计算隐含波动率_American Call
        public static double UsCallOpt_ImpVolatility(double opts, double s, double k, double T, double q, double r, double pankou)
        {
            double tempPankou = pankou / 2;
            //pankou /= 2;
            double m = 0.016;
            double tpvol = m;
            double lastvol = tpvol * 2;
            double y = 0;

            if (opts <= (s - k))
            {
                y = 0;
            }
            else
            {
                double tpopts = UsCallOpt_BiTree(s, k, T, lastvol, q, r);
                if (opts >= tpopts)
                {
                    y = lastvol;
                }
                else
                {
                    for (int ci = 1; ci < 20; ci++)
                    {
                        tpopts = UsCallOpt_BiTree(s, k, T, tpvol, q, r);
                        if (Math.Abs(tpopts - opts) < tempPankou)
                        {
                            y = tpvol;
                            return y; //loop = ci;
                        }
                        else
                        {
                            if (opts > tpopts)
                            {
                                y = tpvol;
                                tpvol = (lastvol + tpvol) / 2;
                            }
                            else
                            {
                                lastvol = tpvol;
                                tpvol = (y + tpvol) / 2;
                            }
                        }
                    }
                }
            }

            return y;
        }

        // 计算隐含波动率_American Put
        public static double UsPutOpt_ImpVolatility(double opts, double s, double k, double T, double q, double r, double pankou)
        {
            double tempPankou = pankou / 2;
            //pankou /= 2;
            double m = 0.016;
            double tpvol = m;
            double lastvol = tpvol * 2;
            double y = 0;

            if (opts <= (k - s))
            {
                y = 0;
            }
            else
            {
                double tpopts = UsPutOpt_BiTree(s, k, T, lastvol, q, r);
                if (opts >= tpopts)
                {
                    y = lastvol;
                }
                else
                {
                    for (int ci = 1; ci < 20; ci++)
                    {
                        tpopts = UsPutOpt_BiTree(s, k, T, tpvol, q, r);
                        if (Math.Abs(tpopts - opts) < tempPankou)
                        {
                            y = tpvol;
                            return y; //loop = ci;
                        }
                        else
                        {
                            if (opts > tpopts)
                            {
                                y = tpvol;
                                tpvol = (lastvol + tpvol) / 2;
                            }
                            else
                            {
                                lastvol = tpvol;
                                tpvol = (y + tpvol) / 2;
                            }
                        }
                    }
                }
            }
            return y;
        }

        // 计算隐含波动率
        public static double GetImpliedVolatility(Contract option, double optPrice, double underlyingPrice, double q, double r, bool isBiTreeUsed = false)
        {
            double sigma = 0.0;
            //Util.Log("Code:" + option.Code);
            Contract futures = CodeSetManager.GetContractInfo(CodeSetManager.GetOptionUnderlyingCode(option, option.ExchCode), option.ExchCode);
            if (futures != null)
            {
                double k = option.Strike;
                int t = CodeSetManager.GetContractRemainingDays(option);

                if (option.ExchCode == "CFFEX" || option.ExchCode == "SHFE") // European Option
                {
                    if (isBiTreeUsed) // 二叉树算法
                    {
                        if (option.OptionType.Contains("Call"))
                        {
                            sigma = EuCallOpt_ImpVolatility_BiTree(optPrice, underlyingPrice, k, t, q, r, (double)option.Fluct);
                        }
                        else if (option.OptionType.Contains("Put"))
                        {
                            sigma = EuPutOpt_ImpVolatility_BiTree(optPrice, underlyingPrice, k, t, q, r, (double)option.Fluct);
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                    }
                    else // B-S公式结果计算
                    {
                        if (option.OptionType.Contains("Call"))
                        {
                            sigma = EuCallOpt_ImpVolatility(optPrice, underlyingPrice, k, t, q, r);
                        }
                        else if (option.OptionType.Contains("Put"))
                        {
                            sigma = EuPutOpt_ImpVolatility(optPrice, underlyingPrice, k, t, q, r);
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                        return sigma;
                    }
                }
                else if (option.ExchCode == "CZCE" || option.ExchCode == "DCE") // American Option
                {
                    if (option.OptionType.Contains("Call"))
                    {
                        sigma = UsCallOpt_ImpVolatility(optPrice, underlyingPrice, k, t, q, r, (double)option.Fluct);
                    }
                    else if (option.OptionType.Contains("Put"))
                    {
                        sigma = UsPutOpt_ImpVolatility(optPrice, underlyingPrice, k, t, q, r, (double)option.Fluct);
                    }
                    else
                    {
                        Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                    }
                }
            }
            else
            {
                Util.Log("Warning! Invalid Futures Contract for the Option " + option.Code + "!");
            }
            if (sigma > 1e+5)
            {
                Util.Log("Warning! Invalid sigma for " + option.Code + ": " + sigma + "!");
                sigma = 0;
            }
            //Util.Log(option.Code + ": sigma = " + sigma);
            return sigma;
        }

        // 计算瞬时delta
        public static double GetInstantDelta(Contract option, double optPrice, double underlyingPrice, double sigma, double q, double r, bool isBiTreeUsed = false)
        {
            double delta = 0;
            double valueUp = 0;
            double valueDown = 0;

            Contract futures = CodeSetManager.GetContractInfo(CodeSetManager.GetOptionUnderlyingCode(option, option.ExchCode), option.ExchCode);
            if (futures != null && underlyingPrice > 0)
            {
                double k = option.Strike;
                double T = CodeSetManager.GetContractRemainingDays(option) ;

                // BiTree Parameter
                double deltaT = 1 ; // Trading days
                double tBiTree = T - deltaT;

                // Futures Price Bias
                double futuresValueUp = underlyingPrice * Math.Exp(sigma * Math.Sqrt(deltaT));
                double futuresValueDown = underlyingPrice * Math.Exp(-sigma * Math.Sqrt(deltaT));
                
                if (option.ExchCode == "CFFEX" || option.ExchCode == "SHFE") // European Option
                {
                    if (isBiTreeUsed) // 二叉树算法
                    {
                        if (option.OptionType.Contains("Call"))
                        {
                            valueUp = EuCallOpt_BiTree(futuresValueUp, k, tBiTree, sigma, q, r);
                            valueDown = EuCallOpt_BiTree(futuresValueDown, k, tBiTree, sigma, q, r);
                        }
                        else if (option.OptionType.Contains("Put"))
                        {
                            valueUp = EuPutOpt_BiTree(futuresValueUp, k, tBiTree, sigma, q, r);
                            valueDown = EuPutOpt_BiTree(futuresValueDown, k, tBiTree, sigma, q, r);
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                    }
                    else // B-S公式结果计算
                    {
                        if (option.OptionType.Contains("Call"))
                        {
                            double d1 = (Math.Log(underlyingPrice / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
                            delta = Math.Exp(-q * T) * NormStdDist(d1);
                        }
                        else if (option.OptionType.Contains("Put"))
                        {
                            double d1 = (Math.Log(underlyingPrice / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
                            delta = Math.Exp(-q * T) * (NormStdDist(d1) - 1);
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                        return delta;
                    }
                }
                else if (option.ExchCode == "CZCE" || option.ExchCode == "DCE") // American Option
                {
                    if (option.OptionType.Contains("Call"))
                    {
                        valueUp = UsCallOpt_BiTree(futuresValueUp, k, tBiTree, sigma, q, r);
                        valueDown = UsCallOpt_BiTree(futuresValueDown, k, tBiTree, sigma, q, r);
                    }
                    else if (option.OptionType.Contains("Put"))
                    {
                        valueUp = UsPutOpt_BiTree(futuresValueUp, k, tBiTree, sigma, q, r);
                        valueDown = UsPutOpt_BiTree(futuresValueDown, k, tBiTree, sigma, q, r);
                    }
                    else
                    {
                        Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                    }
                }

                delta = (valueUp - valueDown) / (futuresValueUp - futuresValueDown);
            }
            else
            {
                Util.Log("Warning in delta! Invalid Futures for the Option " + option.Code + ", price = " +  underlyingPrice);
            }
            //Util.Log(option.Code + ": delta = " + delta);
            return delta;
        }

        // 计算瞬时Gamma
        public static double GetInstantGamma(Contract option, double optPrice, double underlyingPrice, double sigma, double q, double r, bool isBiTreeUsed = false)
        {
            double gamma = 0;
            double valueUp = 0;
            double valueDown = 0;
            double value0 = 0;

            Contract futures = CodeSetManager.GetContractInfo(CodeSetManager.GetOptionUnderlyingCode(option, option.ExchCode), option.ExchCode);
            if (futures != null && underlyingPrice > 0)
            {
                double k = option.Strike;
                double T = CodeSetManager.GetContractRemainingDays(option) ;

                // BiTree Parameters
                double deltaT = 1 ; // Trading days
                double tBiTree = T - 2 * deltaT;
                double u = Math.Exp(sigma * Math.Sqrt(deltaT));
                double d = Math.Exp(-sigma * Math.Sqrt(deltaT));
                double h = 0.5 * underlyingPrice * (u * u - d * d);

                // Futures Price Bias
                double futuresValueUp = underlyingPrice * u * u;
                double futuresValueDown = underlyingPrice * d * d;

                if (option.ExchCode == "CFFEX" || option.ExchCode == "SHFE") // European Option
                {
                    if (isBiTreeUsed) // 二叉树算法
                    {
                        if (option.OptionType.Contains("Call"))
                        {
                            valueUp = EuCallOpt_BiTree(futuresValueUp, k, tBiTree, sigma, q, r);
                            value0 = EuCallOpt_BiTree(underlyingPrice, k, tBiTree, sigma, q, r);
                            valueDown = EuCallOpt_BiTree(futuresValueDown, k, tBiTree, sigma, q, r);
                        }
                        else if (option.OptionType.Contains("Put"))
                        {
                            valueUp = EuPutOpt_BiTree(futuresValueUp, k, tBiTree, sigma, q, r);
                            value0 = EuPutOpt_BiTree(underlyingPrice, k, tBiTree, sigma, q, r);
                            valueDown = EuPutOpt_BiTree(futuresValueDown, k, tBiTree, sigma, q, r);
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                    }
                    else // B-S公式结果计算
                    {
                        if (option.OptionType.Contains("Call") || option.OptionType.Contains("Put"))
                        {
                            double d1 = (Math.Log(underlyingPrice / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
                            double d2 = d1 - sigma * Math.Sqrt(T);
                            double diffNd1 = Math.Exp(-d2 * d2 / 2 - sigma * d2 * Math.Sqrt(T) - sigma * sigma * T / 2) / Math.Sqrt(2 * Math.PI);
                            gamma = diffNd1 * Math.Exp(-q * T) / (underlyingPrice * sigma * Math.Sqrt(T));
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                        return gamma;
                    }
                }
                else if (option.ExchCode == "CZCE" || option.ExchCode == "DCE") // American Option
                {
                    if (option.OptionType.Contains("Call"))
                    {
                        valueUp = UsCallOpt_BiTree(futuresValueUp, k, tBiTree, sigma, q, r);
                        value0 = UsCallOpt_BiTree(underlyingPrice, k, tBiTree, sigma, q, r);
                        valueDown = UsCallOpt_BiTree(futuresValueDown, k, tBiTree, sigma, q, r);
                    }
                    else if (option.OptionType.Contains("Put"))
                    {
                        valueUp = UsPutOpt_BiTree(futuresValueUp, k, tBiTree, sigma, q, r);
                        value0 = UsPutOpt_BiTree(underlyingPrice, k, tBiTree, sigma, q, r);
                        valueDown = UsPutOpt_BiTree(futuresValueDown, k, tBiTree, sigma, q, r);
                    }
                    else
                    {
                        Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                    }
                }
                gamma = ((valueUp - value0) / (underlyingPrice * u * u - underlyingPrice) - (value0 - valueDown) / (underlyingPrice - underlyingPrice * d * d));// *option.Hycs / (h * futures.Hycs);
            }
            else
            {
                Util.Log("Warning in Gamma! Invalid Futures for the Option " + option.Code + ", price = " + underlyingPrice);
            }
            //Util.Log(option.Code + ": gamma = " + gamma);
            return gamma;
        }

        // 计算瞬时vega
        public static double GetInstantVega(Contract option, double optPrice, double underlyingPrice, double sigma, double q, double r, bool isBiTreeUsed = false)
        {
            double vega = 0;
            double valueUp = 0;
            double value0 = 0;

            Contract futures = CodeSetManager.GetContractInfo(CodeSetManager.GetOptionUnderlyingCode(option, option.ExchCode), option.ExchCode);
            if (futures != null && underlyingPrice > 0)
            {
                double k = option.Strike;
                double T = CodeSetManager.GetContractRemainingDays(option) ;

                double sigmaDelta = 0.01;
                double upSigma = sigma + sigmaDelta;

                if (option.ExchCode == "CFFEX" || option.ExchCode == "SHFE") // European Option
                {
                    if (isBiTreeUsed) // 二叉树算法
                    {
                        if (option.OptionType.Contains("Call"))
                        {
                            valueUp = EuCallOpt_BiTree(underlyingPrice, k, T, upSigma, q, r);
                            value0 = EuCallOpt_BiTree(underlyingPrice, k, T, sigma, q, r);
                        }
                        else if (option.OptionType.Contains("Put"))
                        {
                            valueUp = EuPutOpt_BiTree(underlyingPrice, k, T, upSigma, q, r);
                            value0 = EuPutOpt_BiTree(underlyingPrice, k, T, sigma, q, r);
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                    }
                    else // B-S公式结果计算
                    {
                        if (option.OptionType.Contains("Call") || option.OptionType.Contains("Put"))
                        {
                            double d1 = (Math.Log(underlyingPrice / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
                            double d2 = d1 - sigma * Math.Sqrt(T);
                            double diffNd1 = Math.Exp(-d2 * d2 / 2 - sigma * d2 * Math.Sqrt(T) - sigma * sigma * T / 2) / Math.Sqrt(2 * Math.PI);
                            vega = underlyingPrice * Math.Exp(-q * T) * Math.Sqrt(T) * diffNd1 * 0.01; // 1%
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                        return vega;
                    }
                }
                else if (option.ExchCode == "CZCE" || option.ExchCode == "DCE") // American Option
                {
                    if (option.OptionType.Contains("Call"))
                    {
                        valueUp = UsCallOpt_BiTree(underlyingPrice, k, T, upSigma, q, r);
                        value0 = UsCallOpt_BiTree(underlyingPrice, k, T, sigma, q, r);
                    }
                    else if (option.OptionType.Contains("Put"))
                    {
                        valueUp = UsPutOpt_BiTree(underlyingPrice, k, T, upSigma, q, r);
                        value0 = UsPutOpt_BiTree(underlyingPrice, k, T, sigma, q, r);
                    }
                    else
                    {
                        Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                    }
                }
                vega = (valueUp - value0) * 0.01 / (sigmaDelta); // 1%
            }
            else
            {
                Util.Log("Warning in Vega! Invalid Futures for the Option " + option.Code + ", price = " + underlyingPrice);
            }
            //Util.Log(option.Code + ": vega = " + vega);
            return vega;
        }

        // 计算瞬时vega
        public static double GetInstantTheta(Contract option, double optPrice, double underlyingPrice, double sigma, double q, double r, bool isBiTreeUsed = false)
        {
            double theta = 0;
            double value0 = 0;

            Contract futures = CodeSetManager.GetContractInfo(CodeSetManager.GetOptionUnderlyingCode(option, option.ExchCode), option.ExchCode);
            if (futures != null && underlyingPrice > 0)
            {
                double k = option.Strike;
                double T = CodeSetManager.GetContractRemainingDays(option) ;

                // BiTree Parameters
                double deltaT = 1 ; // Trading days
                double tBiTree = T - 2 * deltaT;
                double u = Math.Exp(sigma * Math.Sqrt(deltaT));
                double d = Math.Exp(-sigma * Math.Sqrt(deltaT));

                // Futures Price Bias
                double futuresValueUp = underlyingPrice * u * u;
                double futuresValueDown = underlyingPrice * d * d;

                if (option.ExchCode == "CFFEX" || option.ExchCode == "SHFE") // European Option
                {
                    if (isBiTreeUsed) // 二叉树算法
                    {
                        if (option.OptionType.Contains("Call"))
                        {
                            value0 = EuCallOpt_BiTree(underlyingPrice, k, tBiTree, sigma, q, r);
                        }
                        else if (option.OptionType.Contains("Put"))
                        {
                            value0 = EuPutOpt_BiTree(underlyingPrice, k, tBiTree, sigma, q, r);
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                    }
                    else // B-S公式结果计算
                    {
                        if (option.OptionType.Contains("Call"))
                        {
                            double d1 = (Math.Log(underlyingPrice / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
                            double d2 = d1 - sigma * Math.Sqrt(T);
                            double diffNd1 = Math.Exp(-d2 * d2 / 2 - sigma * d2 * Math.Sqrt(T) - sigma * sigma * T / 2) / Math.Sqrt(2 * Math.PI);
                            theta = -underlyingPrice * Math.Exp(-q * T) * sigma * diffNd1 / (2 * Math.Sqrt(T)) + q * underlyingPrice * Math.Exp(-q * T) * NormStdDist(d1) - r * k * Math.Exp(-r * T) * NormStdDist(d2);
                        }
                        else if (option.OptionType.Contains("Put"))
                        {
                            double d1 = (Math.Log(underlyingPrice / k) + (r - q + sigma * sigma / 2.0) * T) / (sigma * Math.Sqrt(T));
                            double d2 = d1 - sigma * Math.Sqrt(T);
                            double diffNd1 = Math.Exp(-d2 * d2 / 2 - sigma * d2 * Math.Sqrt(T) - sigma * sigma * T / 2) / Math.Sqrt(2 * Math.PI);
                            theta = -underlyingPrice * Math.Exp(-q * T) * sigma * diffNd1 / (2 * Math.Sqrt(T)) - q * underlyingPrice * Math.Exp(-q * T) * NormStdDist(-d1) + r * k * Math.Exp(-r * T) * NormStdDist(-d2);
                        }
                        else
                        {
                            Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                        }
                        return theta;
                    }
                }
                else if (option.ExchCode == "CZCE" || option.ExchCode == "DCE") // American Option
                {
                    if (option.OptionType.Contains("Call"))
                    {
                        value0 = UsCallOpt_BiTree(underlyingPrice, k, tBiTree, sigma, q, r);
                    }
                    else if (option.OptionType.Contains("Put"))
                    {
                        value0 = UsPutOpt_BiTree(underlyingPrice, k, tBiTree, sigma, q, r);
                    }
                    else
                    {
                        Util.Log("Warning! Invalid Option Type! Code:" + option.Code);
                    }
                }
                theta = (value0 - optPrice) / (2 * deltaT);
            }
            else
            {
                Util.Log("Warning in Theta! Invalid Futures for the Option " + option.Code + ", price = " + underlyingPrice);
            }
            return theta;
        }

        //public static bool isOddEven(double price, double factor)
        //{
        //    int num = (int)(Math.Round(price / factor));

        //    if (num % 2 == 1)
        //        return true;
        //    else
        //        return false;
        //}

        public static double UsOption_ControlVariate(double euBiTreeValue, double usBiTreeValue, double bsValue) 
        {
            return usBiTreeValue + bsValue - euBiTreeValue;
        }
    }
}
