namespace GRHelper.Services.Data
{
    using System;
    using System.Text;

    public class HelperMethods
    {
        public static string GeneratePassword(int length)
        {
            {
                const string valid = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ123456789";
                StringBuilder res = new();
                Random rnd = new();
                while (length-- > 0)
                {
                    res.Append(valid[rnd.Next(valid.Length - 1)]);
                }

                return res.ToString();
            }
        }
    }
}
