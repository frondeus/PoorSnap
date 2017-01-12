using System;

namespace BTApplication.Models
{
	public class Message
	{
		//public User User { get; set; } dla 2 uzytkownikow nie potrzebne
		public string TextContent { get; set; }
        public bool isLocal { get; set; }
        public string Name { get; set; }
	}
}
