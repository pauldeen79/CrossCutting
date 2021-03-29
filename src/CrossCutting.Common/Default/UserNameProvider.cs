using CrossCutting.Common.Abstractions;
using System;

namespace CrossCutting.Common.Default
{
    public class UserNameProvider : IUserNameProvider
    {
        public string GetCurrentUserName() =>
            Environment.UserName;
    }
}
