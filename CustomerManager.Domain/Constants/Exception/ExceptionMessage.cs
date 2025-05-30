﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Domain.Constants.Exception
{
    public static class ExceptionMessage
    {
        public const string CONFIG_NOT_FOUND = "Config file not found.";
        public const string DATABASE_NOT_FOUND = "Database not found.";
        public const string NULL_ID_VALUE = "Id value cannot be null.";
        public const string NOT_HAVE_PROPERTY = "Entity does not have a key property.";
        public const string FAILED_INSERTION = "Insert failed, no ID returned.";
    }
}
