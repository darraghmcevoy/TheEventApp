﻿using Microsoft.Owin;
using Owin;
using System.Data.Entity.Migrations;

[assembly: OwinStartupAttribute(typeof(TheEventApp.Startup))]
namespace TheEventApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
