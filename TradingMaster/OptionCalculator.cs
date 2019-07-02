using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TradingMaster.CodeSet;
using TradingMaster.JYData;

namespace TradingMaster
{
    public class OptionCalculator
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static SynQueue<object> _RealDataQueue = new SynQueue<object>();

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
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error("exception: " + ex.StackTrace);
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
                            optionData.Sigma_C = GetImpliedVolatility(callOpt, callRealData.MarketBasePrice, futRealData.MarketBasePrice, 0, 0.000025, true);
                            if (!double.IsInfinity(optionData.Sigma_C) && !double.IsNaN(optionData.Sigma_C) && !double.IsInfinity(optionData.Sigma_C))
                            {
                                optionData.Delta_C = GetInstantDelta(callOpt, callRealData.MarketBasePrice, futRealData.MarketBasePrice, optionData.Sigma_C, q, r);
                                optionData.Gamma_C = GetInstantGamma(callOpt, callRealData.MarketBasePrice, futRealData.MarketBasePrice, optionData.Sigma_C, q, r);
                                optionData.Vega_C = GetInstantVega(callOpt, callRealData.MarketBasePrice, futRealData.MarketBasePrice, optionData.Sigma_C, q, r);
                                optionData.Theta_C = GetInstantTheta(callOpt, callRealData.MarketBasePrice, futRealData.MarketBasePrice, optionData.Sigma_C, q, r);
                            }
                        }
                        if (putRealData != null)
                        {
                            optionData.Sigma_P = GetImpliedVolatility(putOpt, putRealData.NewPrice, futRealData.NewPrice, 0, 0.000025, true);
                            if (!double.IsInfinity(optionData.Sigma_P) && !double.IsNaN(optionData.Sigma_P) && !double.IsInfinity(optionData.Sigma_P))
                            {
                                optionData.Delta_P = GetInstantDelta(putOpt, putRealData.MarketBasePrice, futRealData.MarketBasePrice, optionData.Sigma_P, q, r);
                                optionData.Gamma_P = GetInstantGamma(putOpt, putRealData.MarketBasePrice, futRealData.MarketBasePrice, optionData.Sigma_P, q, r);
                                optionData.Vega_P = GetInstantVega(putOpt, putRealData.MarketBasePrice, futRealData.MarketBasePrice, optionData.Sigma_P, q, r);
                                optionData.Theta_P = GetInstantTheta(putOpt, putRealData.MarketBasePrice, futRealData.MarketBasePrice, optionData.Sigma_P, q, r);
                            }
                        }
                    }
                    else
                    {
                        Util.Log(string.Format("Warning! illegal Contract Quote Data: fCode = {0}", fCode.Code));
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

        private static double[] NORMSCDF01 = {50000000, 50003989, 50007979, 50011968, 50015958, 50019947, 50023937, 50027926, 50031915, 50035905,
                                            50039894, 50043884, 50047873, 50051862, 50055852, 50059841, 50063831, 50067820, 50071810, 50075799,
                                            50079788, 50083778, 50087767, 50091757, 50095746, 50099735, 50103725, 50107714, 50111704, 50115693,
                                            50119683, 50123672, 50127661, 50131651, 50135640, 50139630, 50143619, 50147608, 50151598, 50155587,
                                            50159576, 50163566, 50167555, 50171545, 50175534, 50179523, 50183513, 50187502, 50191492, 50195481,
                                            50199470, 50203460, 50207449, 50211438, 50215428, 50219417, 50223407, 50227396, 50231385, 50235375,
                                            50239364, 50243353, 50247343, 50251332, 50255321, 50259311, 50263300, 50267289, 50271279, 50275268,
                                            50279257, 50283247, 50287236, 50291225, 50295215, 50299204, 50303193, 50307183, 50311172, 50315161,
                                            50319150, 50323140, 50327129, 50331118, 50335108, 50339097, 50343086, 50347075, 50351065, 50355054,
                                            50359043, 50363032, 50367022, 50371011, 50375000, 50378989, 50382979, 50386968, 50390957, 50394946,
                                            50398936, 50402925, 50406914, 50410903, 50414892, 50418882, 50422871, 50426860, 50430849, 50434838,
                                            50438828, 50442817, 50446806, 50450795, 50454784, 50458774, 50462763, 50466752, 50470741, 50474730,
                                            50478719, 50482708};

        private static double[] NORMSCDF02 = {50376388, 50528010, 50644363, 50742453, 50828870, 50906995, 50978836, 51045702, 51108501, 51167896,
                                            51224386, 51278360, 51330125, 51379933, 51427990, 51474468, 51519513, 51563250, 51605786, 51647215,
                                            51687619, 51727070, 51765632, 51803364, 51840316, 51876534, 51912061, 51946934, 51981188, 52014855,
                                            52047965, 52080543, 52112615, 52144202, 52175328, 52206010, 52236268, 52266119, 52295578, 52324660,
                                            52353380, 52381751, 52409785, 52437493, 52464887, 52491977, 52518774, 52545285, 52571521, 52597489,
                                            52623198, 52648654, 52673867, 52698841, 52723584, 52748102, 52772401, 52796487, 52820365, 52844041,
                                            52867519, 52890804, 52913902, 52936816, 52959550, 52982110, 53004498, 53026719, 53048776, 53070673,
                                            53092413, 53114000, 53135436, 53156725, 53177870, 53198873, 53219738, 53240467, 53261063, 53281528,
                                            53301864, 53322075, 53342161, 53362127, 53381973, 53401701, 53421315, 53440815, 53460204, 53479483,
                                            53498655, 53517721, 53536683, 53555543, 53574301, 53592961, 53611522, 53629988, 53648358, 53666636,
                                            53684821, 53702916, 53720921, 53738839, 53756670, 53774415, 53792077, 53809655, 53827151, 53844566,
                                            53861902, 53879159, 53896339, 53913442, 53930469, 53947422, 53964301, 53981107, 53997842, 54014505,
                                            54031099, 54047624, 54064080, 54080469, 54096791, 54113048, 54129239, 54145366, 54161430, 54177431,
                                            54193369, 54209246, 54225063, 54240819, 54256516, 54272155, 54287735, 54303258};

        private static double[] NORMSCDF1 = {53855663, 54316396, 54733821, 55118399, 55476945, 55814198, 56133607, 56437777, 56728732, 57008083,
                                            57277135, 57536965, 57788473, 58032420, 58269458, 58500148, 58724980, 58944382, 59158732, 59368367,
                                            59573585, 59774656, 59971823, 60165304, 60355299, 60541990, 60725542, 60906108, 61083828, 61258832,
                                            61431240, 61601162, 61768702, 61933955, 62097012, 62257956, 62416867, 62573816, 62728874, 62882106,
                                            63033573, 63183333, 63331441, 63477948, 63622904, 63766355, 63908346, 64048918, 64188111, 64325964,
                                            64462512, 64597790, 64731832, 64864669, 64996331, 65126847, 65256245, 65384552, 65511792, 65637991,
                                            65763172, 65887358, 66010569, 66132828, 66254155, 66374569, 66494088, 66612731, 66730515, 66847457,
                                            66963573, 67078880, 67193392, 67307123, 67420090, 67532304, 67643779, 67754529, 67864566, 67973903,
                                            68082550, 68190519, 68297822, 68404470, 68510473, 68615840, 68720583, 68824711, 68928232, 69031158,
                                            69133495, 69235253, 69336441, 69437066, 69537137, 69636661, 69735646, 69834100, 69932030, 70029442,
                                            70126345, 70222743, 70318645, 70414057, 70508984, 70603434, 70697411, 70790922, 70883973, 70976570,
                                            71068716, 71160420, 71251684, 71342515, 71432918, 71522897, 71612458, 71701605, 71790342, 71878675,
                                            71966607, 72054144, 72141289, 72228047, 72314422, 72400417, 72486037, 72571286, 72656167, 72740685,
                                            72824842, 72908643, 72992091, 73075189, 73157941, 73240351, 73322421, 73404156, 73485557, 73566628,
                                            73647373, 73727793, 73807894, 73887676, 73967143, 74046298, 74125144, 74203683, 74281919, 74359853,
                                            74437488, 74514827, 74591873, 74668628, 74745094, 74821273, 74897169, 74972784, 75048119, 75123177,
                                            75197960, 75272471, 75346712, 75420685, 75494391, 75567833, 75641014, 75713934, 75786596, 75859003,
                                            75931155, 76003055, 76074705, 76146106, 76217261, 76288171, 76358837, 76429263, 76499449, 76569397,
                                            76639109, 76708586, 76777831, 76846844, 76915627, 76984183, 77052512, 77120616, 77188497, 77256155,
                                            77323593, 77390812, 77457814, 77524599, 77591170, 77657527, 77723672, 77789607, 77855332, 77920850,
                                            77986161, 78051266, 78116167, 78180866, 78245363, 78309660, 78373757, 78437657, 78501360, 78564868,
                                            78628181, 78691301, 78754229, 78816966, 78879513, 78941872, 79004043, 79066027, 79127826, 79189441,
                                            79250872, 79312121, 79373188, 79434076, 79494783, 79555313, 79615666, 79675842, 79735842, 79795668,
                                            79855321, 79914801, 79974110, 80033248, 80092216, 80151015, 80209646, 80268110, 80326407, 80384539,
                                            80442507, 80500311, 80557951, 80615430, 80672748, 80729905, 80786902, 80843741, 80900421, 80956945,
                                            81013311, 81069522, 81125578, 81181480, 81237229, 81292824, 81348268, 81403560, 81458702, 81513694,
                                            81568536, 81623230, 81677777, 81732176, 81786429, 81840537, 81894499, 81948317, 82001991, 82055522,
                                            82108911, 82162158, 82215264, 82268229, 82321054, 82373740, 82426288, 82478697, 82530969, 82583104,
                                            82635103, 82686966, 82738694, 82790288, 82841748, 82893074, 82944267, 82995329, 83046258, 83097057,
                                            83147725, 83198263, 83248672, 83298951, 83349102, 83399126, 83449021, 83498790, 83548433, 83597950,
                                            83647342, 83696609, 83745751, 83794770, 83843665, 83892438, 83941088, 83989616, 84038023, 84086309,
                                            84134475, 84178618};

        private static double[] NORMSCDF2 = {84134476, 84178619, 84222740, 84266839, 84310915, 84354969, 84398999, 84443007, 84486991, 84530951,
                                            84574888, 84618802, 84662692, 84706558, 84750400, 84794218, 84838011, 84881780, 84925525, 84969245,
                                            85012940, 85056610, 85100256, 85143876, 85187470, 85231040, 85274583, 85318101, 85361593, 85405060,
                                            85448500, 85491913, 85535301, 85578662, 85621996, 85665304, 85708584, 85751838, 85795064, 85838263,
                                            85881435, 85924579, 85967695, 86010783, 86053843, 86096876, 86139880, 86182855, 86225802, 86268720,
                                            86311610, 86354470, 86397301, 86440103, 86482876, 86525619, 86568332, 86611016, 86653670, 86696293,
                                            86738886, 86781449, 86823982, 86866483, 86908954, 86951394, 86993803, 87036181, 87078527, 87120841,
                                            87163124, 87205375, 87247595, 87289782, 87331936, 87374059, 87416148, 87458205, 87500230, 87542221,
                                            87584179, 87626104, 87667995, 87709853, 87751677, 87793467, 87835223, 87876945, 87918632, 87960285,
                                            88001904, 88043487, 88085036, 88126550, 88168028, 88209471, 88250878, 88292250, 88333586, 88374885,
                                            88416149, 88457376, 88498567, 88539721, 88580838, 88621919, 88662962, 88703968, 88744937, 88785868,
                                            88826761, 88867617, 88908434, 88949213, 88989954, 89030656, 89071320, 89111945, 89152531, 89193077,
                                            89233585, 89274053, 89314481, 89354869, 89395218, 89435526, 89475794, 89516022, 89556209, 89596355,
                                            89636461, 89676525, 89716548, 89756529, 89796469, 89836367, 89876224, 89916038, 89955810, 89995539,
                                            90035226, 90074870, 90114472, 90154030, 90193545, 90233016, 90272444, 90311828, 90351168, 90390464,
                                            90429716, 90468924, 90508086, 90547204, 90586277, 90625305, 90664288, 90703225, 90742116, 90780962,
                                            90819762, 90858515, 90897222, 90935883, 90974497, 91013064, 91051584, 91090057, 91128482, 91166860,
                                            91205190, 91243473, 91281707, 91319893, 91358030, 91396119, 91434159, 91472150, 91510092, 91547984,
                                            91585827, 91623620, 91661364, 91699057, 91736700, 91774292, 91811834, 91849326, 91886766, 91924155,
                                            91961492, 91998779, 92036013, 92073195, 92110326, 92147404, 92184430, 92221403, 92258323, 92295190,
                                            92332004, 92368765, 92405472, 92442125, 92478724, 92515269, 92551760, 92588196, 92624578, 92660904,
                                            92697176, 92733392, 92769553, 92805658, 92841707, 92877700, 92913637, 92949518, 92985341, 93021108,
                                            93056818, 93092471, 93128066, 93163604, 93199084, 93234505, 93269869, 93305174, 93340421, 93375609,
                                            93410737, 93445807, 93480817, 93515768, 93550659, 93585489, 93620260, 93654970, 93689620, 93724209,
                                            93758736, 93793203, 93827608, 93861952, 93896233, 93930453, 93964610, 93998705, 94032738, 94066707,
                                            94100614, 94134457, 94168237, 94201953, 94235605, 94269193, 94302717, 94336176, 94369570, 94402900,
                                            94436164, 94469364, 94502497, 94535565, 94568567, 94601502, 94634371, 94667174, 94699909, 94732578,
                                            94765179, 94797713, 94830179, 94862577, 94894907, 94927169, 94959362, 94991486, 95023542, 95055528,
                                            95087444, 95119291, 95151068, 95182775, 95214412, 95245978, 95277473, 95308898, 95340251, 95371532,
                                            95402742, 95433880, 95464946, 95495940, 95526861, 95557709, 95588484, 95619186, 95649814, 95680369,
                                            95710850, 95741257, 95771589, 95801846, 95832029, 95862137, 95892169, 95922126, 95952007, 95981812,
                                            96011541, 96041193, 96070768, 96100267, 96129688, 96159032, 96188299, 96217487, 96246597, 96275629,
                                            96304582, 96333457, 96362252, 96390968, 96419604, 96448161, 96476637, 96505033, 96533349, 96561584,
                                            96589737, 96617810, 96645801, 96673710, 96701537, 96729282, 96756944, 96784524, 96812020, 96839433,
                                            96866763, 96894009, 96921171, 96948249, 96975242, 97002150, 97028973, 97055711, 97082364, 97108931,
                                            97135411, 97161805, 97188113, 97214334, 97240468, 97266514, 97292473, 97318344, 97344127, 97369821,
                                            97395427, 97420944, 97446371, 97471710, 97496958, 97522117, 97547185, 97572162, 97597049, 97621845,
                                            97646549, 97671162, 97695683, 97720112, 97744449, 97768692, 97792843, 97816900, 97840864, 97864734,
                                            97888510, 97912192, 97935779, 97959271, 97982668, 98005970, 98029175, 98052285, 98075298, 98098215,
                                            98121034, 98143757, 98166382, 98188909, 98211338, 98233669, 98255901, 98278035, 98300069, 98322003,
                                            98343838, 98365572, 98387207, 98408740, 98430172, 98451504, 98472733, 98493861, 98514886, 98535809,
                                            98556629, 98577346, 98597960, 98618470, 98638876, 98659177, 98679374, 98699466, 98719453, 98739334,
                                            98759110, 98778779, 98798342, 98817798, 98837146, 98856388, 98875522, 98894547, 98913465, 98932273,
                                            98950973, 98969564, 98988045, 99006416, 99024677, 99042828, 99060867, 99078796, 99096613, 99114319,
                                            99131913, 99149394, 99166763, 99184019, 99201162, 99218192, 99235108, 99251910, 99268599, 99285172,
                                            99301631, 99317976, 99334205, 99350319, 99366318, 99382201, 99397968, 99413620, 99429155, 99444575,
                                            99459878, 99475066, 99490137, 99505092, 99519932, 99534656, 99549264, 99563758, 99578136, 99592401,
                                            99606552, 99620591, 99634518, 99648334, 99662041, 99675640, 99689134, 99702524, 99715814, 99729007,
                                            99742107, 99755120, 99768050, 99780908, 99793701, 99806442, 99819148, 99831840, 99844546, 99857305,
                                            99870177, 99883249, 99896670, 99910712};

        private static double NormStdDist(double d)
        {
            try
            {
                double cd = 0.0;
                double p = 0.0;
                if (d < 0)
                {
                    cd = -d;
                }
                else
                {
                    cd = d;
                }
                if (cd > 3.123)
                {
                    cd = 3.123;
                }
                if (cd > 1)
                {
                    cd = cd - 1;
                    cd = cd = cd * (23.2 * Math.Pow(cd, 2) - 198 * cd + 548);//如果是从0开始的索引，此处是 cd=cd*(23.2*cd^2-198*cd+548)
                    int slot = (int)Math.Floor(cd);
                    double ratio = cd - slot;
                    p = (1 - ratio) * NORMSCDF2[slot] + ratio * NORMSCDF2[slot + 1];
                }
                else
                {
                    if (cd > 0.108)
                    {
                        cd = cd * cd * (439 - 125 * cd) - 4;//matlab数组索引从1开始计数，如果是从0开始的索引，此处是 cd= cd*cd*(439-125*cd)-4
                        int slot = (int)Math.Floor(cd);
                        double ratio = cd - slot;
                        p = (1 - ratio) * NORMSCDF1[slot] + ratio * NORMSCDF1[slot + 1];
                    }
                    else
                    {
                        if (cd > 0.012)
                        {
                            cd = cd * (11870 * cd - 6) - 1; //matlab数组索引从1开始计数，如果是从0开始的索引，此处是 cd= cd*(11870*cd-6)-1
                            int slot = (int)Math.Floor(cd);
                            double ratio = cd - slot;
                            p = (1 - ratio) * NORMSCDF02[slot] + ratio * NORMSCDF02[slot + 1];
                        }
                        else
                        {
                            cd = (int)Math.Max(cd * 10000, 0);//matlab数组索引从1开始计数，如果是从0开始的索引，此处是cd= max(cd*10000,0)
                            int slot = (int)Math.Floor(cd);
                            double ratio = cd - slot;
                            p = (1 - ratio) * NORMSCDF01[slot] + ratio * NORMSCDF01[slot + 1];
                        }
                    }
                }
                p = p * Math.Pow(10, -8);
                if (d < 0)
                {
                    p = 1 - p;
                }
                return p;
            }
            catch (Exception ex)
            {
                Util.Log_Error(ex.Message);
                Util.Log_Error(ex.StackTrace);
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
                Util.Log(string.Format("sigma = {0}, calError = {1}, step = {2}", sigma, calError, (EuCallOpt_BsFormula(s0, k, T, sigma, q, r) - bsOptVal) / (s0 * Math.Sqrt(T) * diffNd1 * Math.Exp(-q * T))));
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
                Util.Log(string.Format("sigma = {0}, calError = {1}, step = {2}", sigma, calError, (EuPutOpt_BsFormula(s0, k, T, sigma, q, r) - bsOptVal) / (s0 * Math.Sqrt(T) * diffNd1 * Math.Exp(-q * T))));
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
        public static double EuCallOpt_ImpVolatility_BiTree(double opts, double s, double k, double T, double q, double r, double pankou)
        {
            double pk = pankou / 2;
            double m = 0.08 - Math.Sqrt(T) / 480.0;
            double vr = 0.0;
            double iv = 0.0;
            double pc = 0.0;

            if (opts <= Math.Max(0, s - k))//看涨合约判断条件
            {
                iv = 0;
                return iv;//低于内在价值按0计算，函数输出结果 iv
            }
            else
            {
                pc = EuCallOpt_BiTree(s, k, T, m, q, r);//波动率上限价格
                if (opts >= pc - pankou / 2)
                {
                    iv = m;
                    return iv; //高于波动率上限按上限计算，函数输出结果 iv
                }
                else
                {
                    iv = m / 2;
                    vr = 0;
                    for (int i = 1; i <= 17; i++)
                    {
                        pc = UsCallOpt_BiTree(s, k, T, iv, q, r);
                        //根据看涨看跌和配置的模型选择公式
                        if (Math.Abs(pc - opts) < pankou / 2)
                        {
                            return iv; //得到计算结果，函数输出结果 iv
                        }
                        else
                        {
                            if (opts > pc)
                            {
                                vr = iv;
                                iv = (iv + m) / 2;
                            }
                            else
                            {
                                m = iv;
                                iv = (vr + iv) / 2;
                            }
                        }
                    }
                }
            }
            return iv;
        }

        // 根据报价算隐含波动率_European Put
        public static double EuPutOpt_ImpVolatility_BiTree(double opts, double s, double k, double T, double q, double r, double pankou)
        {
            double pk = pankou / 2;
            double m = 0.08 - Math.Sqrt(T) / 480.0;
            double vr = 0.0;
            double iv = 0.0;
            double pc = 0.0;

            if (opts <= Math.Max(0, k - s))//看跌合约判断条件
            {
                iv = 0;
                return iv;//低于内在价值按0计算，函数输出结果 iv
            }
            else
            {
                pc = UsPutOpt_BiTree(s, k, T, m, q, r);//波动率上限价格
                if (opts >= pc - pankou / 2)
                {
                    iv = m;
                    return iv; //高于波动率上限按上限计算，函数输出结果 iv
                }
                else
                {
                    iv = m / 2;
                    vr = 0;
                    for (int i = 1; i <= 17; i++)
                    {
                        pc = UsPutOpt_BiTree(s, k, T, iv, q, r);
                        //根据看涨看跌和配置的模型选择公式
                        if (Math.Abs(pc - opts) < pankou / 2)
                        {
                            return iv; //得到计算结果，函数输出结果 iv
                        }
                        else
                        {
                            if (opts > pc)
                            {
                                vr = iv;
                                iv = (iv + m) / 2;
                            }
                            else
                            {
                                m = iv;
                                iv = (vr + iv) / 2;
                            }
                        }
                    }
                }
            }
            return iv;
        }

        // 根据二叉树计算报价_European Call
        public static double EuCallOpt_BiTree(double s, double k, double T, double sigma, double q, double r)
        {
            double y = 0;
            int stepnum = (int)Math.Ceiling(10 + 2000 * sigma + 0.00125 * s + 4 * Math.Sqrt(T));

            if (T == 0 || stepnum <= 0)
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
            int stepnum = (int)Math.Ceiling(10 + 2000 * sigma + 0.00125 * s + 4 * Math.Sqrt(T));

            if (T == 0 || stepnum <= 0)
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
                {
                    for (int cj = 0; cj <= ci; cj++)
                    {
                        optionmat[cj][ci] = (optionmat[cj][ci + 1] * p + optionmat[cj + 1][ci + 1] * (1 - p)) / Math.Exp(R);

                        //if ((k - valuemat[cj][ci]) > optionmat[cj][ci])
                        //    optionmat[cj][ci] = k - valuemat[cj][ci];
                    }
                }
                y = optionmat[0][0];
            }
            return y;
        }

        // 根据二叉树计算报价_American Call
        public static double UsCallOpt_BiTree(double s, double k, double T, double sigma, double q, double r)
        {
            double y = 0;
            int stepnum = (int)Math.Ceiling(10 + 2000 * sigma + 0.00125 * s + 4 * Math.Sqrt(T));

            if (T == 0 || stepnum <= 0)
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
            int stepnum = (int)Math.Ceiling(10 + 2000 * sigma + 0.00125 * s + 4 * Math.Sqrt(T));

            if (T == 0 || stepnum <= 0)
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
                {
                    for (int cj = 0; cj <= ci; cj++)
                    {
                        optionmat[cj][ci] = (optionmat[cj][ci + 1] * p + optionmat[cj + 1][ci + 1] * (1 - p)) / Math.Exp(R);

                        if ((k - valuemat[cj][ci]) > optionmat[cj][ci])
                            optionmat[cj][ci] = k - valuemat[cj][ci];
                    }
                }
                y = optionmat[0][0];
            }
            return y;
        }

        // 计算隐含波动率_American Call
        public static double UsCallOpt_ImpVolatility(double opts, double s, double k, double T, double q, double r, double pankou)
        {
            double pk = pankou / 2;
            double m = 0.08 - Math.Sqrt(T) / 480.0;
            double vr = 0.0;
            double iv = 0.0;
            double pc = 0.0;

            if (opts <= Math.Max(0, s - k))//看涨合约判断条件
            {
                iv = 0;
                return iv;//低于内在价值按0计算，函数输出结果 iv
            }
            else
            {
                pc = UsCallOpt_BiTree(s, k, T, m, q, r);//波动率上限价格
                if (opts >= pc - pankou / 2)
                {
                    iv = m;
                    return iv; //高于波动率上限按上限计算，函数输出结果 iv
                }
                else
                {
                    iv = m / 2;
                    vr = 0;
                    for (int i = 1; i <= 17; i++)
                    {
                        pc = UsCallOpt_BiTree(s, k, T, iv, q, r);
                        //根据看涨看跌和配置的模型选择公式
                        if (Math.Abs(pc - opts) < pankou / 2)
                        {
                            return iv; //得到计算结果，函数输出结果 iv
                        }
                        else
                        {
                            if (opts > pc)
                            {
                                vr = iv;
                                iv = (iv + m) / 2;
                            }
                            else
                            {
                                m = iv;
                                iv = (vr + iv) / 2;
                            }
                        }
                    }
                }
            }
            return iv;
        }

        // 计算隐含波动率_American Put
        public static double UsPutOpt_ImpVolatility(double opts, double s, double k, double T, double q, double r, double pankou)
        {
            double pk = pankou / 2;
            double m = 0.08 - Math.Sqrt(T) / 480.0;
            double vr = 0.0;
            double iv = 0.0;
            double pc = 0.0;

            if (opts <= Math.Max(0, k - s))//看跌合约判断条件
            {
                iv = 0;
                return iv;//低于内在价值按0计算，函数输出结果 iv
            }
            else
            {
                pc = UsPutOpt_BiTree(s, k, T, m, q, r);//波动率上限价格
                if (opts >= pc - pankou / 2)
                {
                    iv = m;
                    return iv; //高于波动率上限按上限计算，函数输出结果 iv
                }
                else
                {
                    iv = m / 2;
                    vr = 0;
                    for (int i = 1; i <= 17; i++)
                    {
                        pc = UsPutOpt_BiTree(s, k, T, iv, q, r);
                        //根据看涨看跌和配置的模型选择公式
                        if (Math.Abs(pc - opts) < pankou / 2)
                        {
                            return iv; //得到计算结果，函数输出结果 iv
                        }
                        else
                        {
                            if (opts > pc)
                            {
                                vr = iv;
                                iv = (iv + m) / 2;
                            }
                            else
                            {
                                m = iv;
                                iv = (vr + iv) / 2;
                            }
                        }
                    }
                }
            }
            return iv;
        }

        // 计算隐含波动率
        public static double GetImpliedVolatility(Contract option, double optPrice, double underlyingPrice, double q, double r, bool isBiTreeUsed = false)
        {
            double sigma = 0.0;
            //Util.Log("Code: "  + option.Code);
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                        Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                double T = CodeSetManager.GetContractRemainingDays(option);

                // BiTree Parameter
                double deltaT = 1; // Trading days
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                        Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
                    }
                }

                delta = (valueUp - valueDown) / (futuresValueUp - futuresValueDown);
            }
            else
            {
                Util.Log("Warning in delta! Invalid Futures for the Option " + option.Code + ", price = " + underlyingPrice);
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
                double T = CodeSetManager.GetContractRemainingDays(option);

                // BiTree Parameters
                double deltaT = 1; // Trading days
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                        Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                double T = CodeSetManager.GetContractRemainingDays(option);

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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                        Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                double T = CodeSetManager.GetContractRemainingDays(option);

                // BiTree Parameters
                double deltaT = 1; // Trading days
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                            Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
                        Util.Log("Warning! Invalid Option Type! Code: "  + option.Code);
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
