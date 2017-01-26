using System.Collections.Generic;

namespace TradingMaster
{
    public class BankManager
    {
        public static List<ContractBank> ContractBanks = new List<ContractBank>();

        public static string GetBankIdFromName(string bankName)
        {
            if (bankName == null) return null;
            foreach (ContractBank bItem in ContractBanks)
            {
                if (bankName.Contains(bItem.BankName))
                {
                    return bItem.BankID;
                }
            }
            return string.Empty;
        }

        public static string GetBankNameFromBankID(string bankID)
        {
            if (bankID == null) return null;
            foreach (ContractBank bItem in ContractBanks)
            {
                if (bankID == bItem.BankID)
                {
                    return bItem.BankName;
                }
            }
            return string.Empty;
        }

        public static string GetBankBranchIdFromName(string bankName)
        {
            if (bankName == null) return null;
            foreach (ContractBank bItem in ContractBanks)
            {
                if (bankName.Contains(bItem.BankName))
                {
                    return bItem.BankBrchID;
                }
            }
            return string.Empty;
        }
    }
}
