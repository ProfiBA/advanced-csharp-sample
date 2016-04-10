using System;
using System.Collections.Generic;

namespace ConsoleApp.code.model
{
    public class Plugin
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string Hash { get; set; }
        public State Status { get; set; }

        public enum State { Update, New }
    }

    public class RootResponse
    {
        public bool Success { get; set; }
        public List<Plugin> Data { get; set; }
        public string ErrMessage { get; set; }
    }
}
