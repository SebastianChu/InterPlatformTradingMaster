using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Data.SqlClient;
using System.Data;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "HFTrader.exe.config", Watch = true)]
namespace AutoTrader
{
    class Program
    {
        public static bool InPrintMode = false;

        static void Main(string[] args)
        {
            //Util.GetLoggers();
            Util.WriteInfo("HFTrader 开始");
            TradeDataServer tradeDataServer = new TradeDataServer();

            while (true)
            {
                string answer = Console.ReadLine();
                if (answer.ToUpper() == "Q")
                {
                    Util.WriteInfo("是否确认退出程序？（Y/N）：");
                    string confirm = Console.ReadLine();
                    if (confirm.ToUpper() == "Y")
                    {
                        Util.WriteInfo("继续按回车直接退出程序...");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (answer.ToUpper() == "P")
                {
                    if (InPrintMode)
                    {
                        InPrintMode = false;
                        Util.WriteInfo("退出循环打印模式");
                    }
                    else
                    {
                        InPrintMode = true;
                        Util.WriteInfo("进入循环打印模式...");
                    }
                    continue;
                }
                else if (answer.ToUpper() == "L")
                {
                    Util.WriteInfo("开始加载新参数...");
                    tradeDataServer.ParseStrategyXmlParameter(AppDomain.CurrentDomain.BaseDirectory + Util.ConfigFile);
                    tradeDataServer.RenewStrategyStatus();
                    Util.WriteInfo("新参数加载完成！");
                    continue;
                }
            }
            //Console.ReadLine();
            //Console.ReadLine();
        }

        static void ExitTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var dtNow = DateTime.Now;
                if (dtNow.Hour > 15)
                {
                    Util.WriteInfo("程序退出..");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Util.WriteExceptionToLogFile(ex);
            }
        }

        static void CheckTradingTime(SqlConnection conn)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                DataTable dataTable = new DataTable();
                string commandText = "SELECT [AssetCode],[ExchangeCode],[DayOpen],[DayBreak],[DayBreakEnd],[NoonBreak],[NoonOpen],[DayClose],[NightTrading],[NightOpen],[NightClose] FROM [HFTrader].[dbo].[tb_TradingSchedule]";
                adapter.SelectCommand = new SqlCommand(commandText, conn);
                adapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {
                        string instrumentId = row[0].ToString();
                        TradingTime tradingTime = new TradingTime();
                        tradingTime.DayOpen = DateTime.Parse(row[2].ToString());
                        if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        {
                            tradingTime.DayBreak = DateTime.Parse(row[3].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(row[4].ToString()))
                        {
                            tradingTime.DayBreakEnd = DateTime.Parse(row[4].ToString());
                        }
                        tradingTime.NoonBreak = DateTime.Parse(row[5].ToString());
                        tradingTime.NoonOpen = DateTime.Parse(row[6].ToString());
                        tradingTime.DayClose = DateTime.Parse(row[7].ToString());
                        tradingTime.NightTrading = int.Parse(row[8].ToString()) == 1 ? true : false;
                        if (tradingTime.NightTrading)
                        {
                            tradingTime.NightOpen = DateTime.Parse(row[9].ToString());
                            tradingTime.NightClose = DateTime.Parse(row[10].ToString());
                        }
                        TradingTimeManager.InstrumentTradingTimeDic[instrumentId] = tradingTime;
                    }
                    catch (Exception ex)
                    {
                        Util.WriteExceptionToLogFile(ex);
                    }
                }
            }
        }

        static void GetStrategyThreshold(SqlConnection conn, TradeDataServer tradeDataServer)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                DataTable dataTable = new DataTable();
                string commandText = "SELECT [Strategy],[AssetCode],[BackAheadRatio],[BackMovementRatio],[MovementVolumeRatio],[MinBlockSize],[MinMovementSignal],[MovementRatioHigh],[MovementRatioLow] ,[LowHeatDurationSignal],[LowVolumeSignal],[MaxTotalHeat],[HeatDuration],[PendingEndSeconds],[EndingSeconds] FROM [HFTrader].[dbo].[tb_StrategyThreshold]";
                adapter.SelectCommand = new SqlCommand(commandText, conn);
                adapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {
                        string instrumentId = row[1].ToString();
                        //StrategyExample strategy = new StrategyExample(instrumentId, tradeDataServer);
                        //strategy.BackAheadRatio = double.Parse(row[2].ToString());
                        //strategy.BackMovementRatio = double.Parse(row[3].ToString());
                        //strategy.MovementVolumeRatio = double.Parse(row[4].ToString());
                        //strategy.MinBlockSize = int.Parse(row[5].ToString());
                        //strategy.MinMovementSignal = int.Parse(row[6].ToString());
                        //strategy.MovementRatioHigh = double.Parse(row[7].ToString());
                        //strategy.MovementRatioLow = double.Parse(row[8].ToString());
                        //strategy.LowHeatDurationSignal = int.Parse(row[9].ToString());
                        //strategy.LowVolumeSignal = int.Parse(row[10].ToString());
                        //strategy.MaxTotalHeat = int.Parse(row[11].ToString());
                        //strategy.HeatDuration = int.Parse(row[12].ToString());
                        //strategy.PendingEndSeconds = int.Parse(row[13].ToString());
                        //strategy.EndingSeconds = int.Parse(row[14].ToString());
                        //Constant.StrategyMap[instrumentId] = strategy;

                    }
                    catch (Exception ex)
                    {
                        Util.WriteExceptionToLogFile(ex);
                    }
                }
            }
        }

    }
}
