namespace MyMmo.Commons {
    public enum ReturnCode : short {

        Ok = 0,
        
        /// <summary>
        /// Operation has been called in wrong state
        /// </summary>
        OperationIsNotAvailable,

        /// <summary>
        /// Operation code is not implemented or supported
        /// </summary>
        OperationIsNotSupported,

        /// <summary>
        /// Operation parameters is invalid, see DebugMessage for details
        /// </summary>
        InvalidOperationParameters,

        /// <summary>
        /// World with name specified is not found
        /// </summary>
        WorldNotFound,

        /// <summary>
        /// World with name specified is already exist
        /// </summary>
        WorldAlreadyExist,

        InternalException

    }
}