using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Response {
    public class EnterWorldResponse {

        [PropertyKey(Key = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string AvatarItemId { get; set; }

    }
}