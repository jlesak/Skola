﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TigersProject.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("character set=NONE;data source=localhost;initial catalog=D:\\DATABASE.FDB;user id=" +
            "sysdba;Password=masterkey;server type=1;client library=D:\\GitHub\\DMP\\TigersProje" +
            "ct\\bin\\fbclient.dll")]
        public string ConnectionStringMyDB {
            get {
                return ((string)(this["ConnectionStringMyDB"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("character set=UTF8;data source=localhost;initial catalog=D:\\GitHub\\DMP\\TIGERS.FDB" +
            ";user id=sysdba;Password=masterkey;server type=1;client library=D:\\GitHub\\DMP\\Ti" +
            "gersProject\\bin\\fbclient.dll")]
        public string ConnectionString {
            get {
                return ((string)(this["ConnectionString"]));
            }
        }
    }
}
