using System;
using MyMmo.Commons.Scripts;

namespace MyMmo.Client.Scripts {
    public static class ClientScriptsFactory {
        
        public static IClientScript Create(BaseScriptData baseScriptData) {
            if (baseScriptData is ChangeLocationScriptData changeLocationScriptData) {
                return new ChangeLocationClientScript(changeLocationScriptData);
            } else if (baseScriptData is ChangePositionScriptData changePositionScriptData) {
                return new ChangePositionClientScript(changePositionScriptData);
            } else {
                throw new Exception("No factory found for scriptData: " + baseScriptData);
            }
        }

    }
}