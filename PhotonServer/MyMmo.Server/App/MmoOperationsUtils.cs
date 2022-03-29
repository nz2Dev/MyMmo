using MyMmo.Commons;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.App {
    public class MmoOperationsUtils {

        public static OperationResponse OperationWrongState(OperationRequest operationRequest) {
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short) ReturnCode.OperationIsNotAvailable,
                DebugMessage = $"Operation {operationRequest.OperationCode} is not available in this state"
            };
        }

        public static OperationResponse OperationNotSupported(OperationRequest operationRequest) {
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short) ReturnCode.OperationIsNotSupported,
                DebugMessage = $"Operation {operationRequest.OperationCode} is not implemented or supported"
            };
        }

        public static OperationResponse OperationWrongDataContract(OperationRequest operationRequest,
            DataContract dataContract) {
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short) ReturnCode.InvalidOperationParameters,
                DebugMessage = dataContract.GetErrorMessage()
            };
        }

        public static OperationResponse OperationSuccess(OperationRequest operationRequest) {
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short) ReturnCode.Ok,
                DebugMessage = "Ok"
            };
        }
        
        public static OperationResponse OperationSuccess(OperationRequest operationRequest, DataContract responseParams) {
            return new OperationResponse(operationRequest.OperationCode, responseParams) {
                ReturnCode = (short) ReturnCode.Ok,
                DebugMessage = "Ok"
            };
        }

        public static OperationResponse OperationError(OperationRequest request, ReturnCode returnCode,
            string debugMessage) {
            return new OperationResponse(request.OperationCode) {
                ReturnCode = (short) returnCode,
                DebugMessage = debugMessage
            };
        }

    }
}