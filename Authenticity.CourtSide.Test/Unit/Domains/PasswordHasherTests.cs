using Authenticity.CourtSide.Core.Domains.Implementation;
using Authenticity.CourtSide.Core.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Domains
{
    [Trait("UnitTest", "PasswordHasher")]
    public class PasswordHasherTests
    {
        #region [Setup]

        private const string SOME_CORRECT_PASSWORD = "NSerio.1";
        private const string SOME_CORRECT_PASSWORD_HASH = "10000.78PY5YtJ9nzsNwTemYVbLw==.p+EGhzbNdCDfNnmqZ0UkF1TrHY6pnzsYsX0XQxRYY0w=";

        private PasswordHasher InstanceToTest { get; set; }

        public PasswordHasherTests()
        {
            var settings = Options.Create(new HashingOptions());
            InstanceToTest = new PasswordHasher(settings);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task Hash_SuccessTestAsync()
        {
            await Task.Yield();
            var result = InstanceToTest.Hash(SOME_CORRECT_PASSWORD);

            Assert.NotEmpty(result);
        }

        #endregion
    }
}
