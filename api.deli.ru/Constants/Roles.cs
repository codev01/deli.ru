﻿using api.deli.ru.Controllers;

namespace api.deli.ru.Constants
{
    public static class Roles
    {
        public const string All = $"{Organization}, {Landlord}, {Tenant}";
        public const string Organization = "organization";
        public const string Landlord = "landlord";
        public const string Tenant = "tenant";
    }
}
