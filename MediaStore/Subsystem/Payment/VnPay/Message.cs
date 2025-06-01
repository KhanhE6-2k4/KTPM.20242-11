namespace MediaStore.Subsystem.VnPay
{
    public static class Message
    {
        private static readonly Dictionary<string, string> messages = new()
        {
            { "00", "Transaction successful." },
            { "07", "Money deducted successfully. Transaction suspected of fraud or unusual activity." },
            { "09", "Transaction failed: Card/Account not registered for Internet Banking." },
            { "10", "Transaction failed: Incorrect card/account authentication information entered more than 3 times." },
            { "11", "Transaction failed: Payment timeout. Please try again." },
            { "12", "Transaction failed: Card/Account is locked." },
            { "13", "Transaction failed: Incorrect OTP entered. Please try again." },
            { "24", "Transaction failed: Customer canceled the transaction." },
            { "51", "Transaction failed: Insufficient balance." },
            { "65", "Transaction failed: Exceeded daily transaction limit." },
            { "75", "Transaction failed: Bank is under maintenance." },
            { "79", "Transaction failed: Payment password entered incorrectly too many times. Please try again." },
            { "99", "Transaction failed: Other errors not listed." }
        };

        public static string GetMessage(string code)
        {
            return messages.TryGetValue(code, out var message) ? message : "Unknown error.";
        }

    }
}