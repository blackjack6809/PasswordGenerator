using AntDesign;
using ConstantPasswordGenerator.ViewModel;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ConstantPasswordGenerator.Pages
{
    public partial class Index
    {
        private string GeneratedPassword = string.Empty;

        Form<GeneratePasswordViewModel> form;
        private GeneratePasswordViewModel model = new GeneratePasswordViewModel();

        private void OnFinish(EditContext editContext)
        {
            Console.WriteLine($"Success:{JsonSerializer.Serialize(model)}");
            GeneratePassword(editContext.Model as GeneratePasswordViewModel);
        }

        private void OnFinishFailed(EditContext editContext)
        {
            Console.WriteLine($"Failed:{JsonSerializer.Serialize(model)}");
        }

        private void GeneratePassword(GeneratePasswordViewModel formData)
        {
            if (!GetRangeValue(formData.Range, out List<int> rangeValues))
            {
                return;
            }

            var data = GetSha512Input(formData);
            var result = GetHashedPassword(data);
            GeneratedPassword = result.Substring(rangeValues.First(), rangeValues.Last());
        }

        private string GetHashedPassword(string data)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private string GetSha512Input(GeneratePasswordViewModel formData)
        {
            return $"{formData.Domain}{formData.Email}{formData.Password}{formData.Step}";
        }

        private bool GetRangeValue(string range, out List<int> rangeValues)
        {
            rangeValues = new List<int>();
            var split = range.Split(':');
            if (split.Length != 2
                || !Int32.TryParse(split.First(), out int first)
                || !Int32.TryParse(split.Last(), out int second)
                )
            {
                return false;
            }

            rangeValues.Add(first);
            rangeValues.Add(second);
            return true;
        }

        //private async Task CopyTextToClipboard()
        //{
        //    await JSRuntime.InvokeVoidAsync("clipboardCopy.copyText", GeneratedPassword);
        //}

    }
}
