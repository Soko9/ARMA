namespace UserManagementApi.Constants
{
    public class Messages
    {
        #region Success
        private static string _CreateSuccess(string Entity) => $"{Entity} Created Successfully.";
        private static string _UpdateSuccess(string Entity) => $"{Entity} Updated Successfully.";
        private static string _DeleteSuccess(string Entity) => $"{Entity} Deleted Successfully.";
        private static string _ToggleSuccess(string Entity) => $"{Entity} Toggled Successfully.";
        #endregion

        #region Error
        private static string _CreateError(string Entity) => $"Error Creating {Entity}";
        private static string _UpdateError(string Entity) => $"Error Updating {Entity}";
        private static string _DeleteError(string Entity) => $"Error Deleting {Entity}";
        private static string _ToggleError(string Entity) => $"Error Toggling {Entity}";
        #endregion

        public static string NotFound(string Entity) => $"{Entity} Not Found.";
        public static string Mismatch() => "ID Mismatch Between Route And Body.";

        #region Permission Category
        public static string PCCreateSuccess() => _CreateSuccess("Permission Category");
        public static string PCUpdateSuccess() => _UpdateSuccess("Permission Category");
        public static string PCDeleteSuccess() => _DeleteSuccess("Permission Category");
        public static string PCToggleSuccess() => _ToggleSuccess("Permission Category");
        public static string PCCreateError() => _CreateError("Permission Category");
        public static string PCUpdateError() => _UpdateError("Permission Category");
        public static string PCDeleteError() => _DeleteError("Permission Category");
        public static string PCToggleError() => _ToggleError("Permission Category");
        #endregion

        #region Permission
        public static string PCreateSuccess() => _CreateSuccess("Permission");
        public static string PUpdateSuccess() => _UpdateSuccess("Permission");
        public static string PDeleteSuccess() => _DeleteSuccess("Permission");
        public static string PToggleSuccess() => _ToggleSuccess("Permission");
        public static string PCreateError() => _CreateError("Permission");
        public static string PUpdateError() => _UpdateError("Permission");
        public static string PDeleteError() => _DeleteError("Permission");
        public static string PToggleError() => _ToggleError("Permission");
        #endregion

        #region Role
        public static string RCreateSuccess() => _CreateSuccess("Role");
        public static string RUpdateSuccess() => _UpdateSuccess("Role");
        public static string RDeleteSuccess() => _DeleteSuccess("Role");
        public static string RToggleSuccess() => _ToggleSuccess("Role");
        public static string RCreateError() => _CreateError("Role");
        public static string RUpdateError() => _UpdateError("Role");
        public static string RDeleteError() => _DeleteError("Role");
        public static string RToggleError() => _ToggleError("Role");
        #endregion

        #region Roles Permissions
        public static string RPCreateSuccess() => _CreateSuccess("Role Permission");
        public static string RPDeleteSuccess() => _DeleteSuccess("Role Permission");
        public static string RPToggleSuccess() => _ToggleSuccess("Role Permission");
        public static string RPCreateError() => _CreateError("Role Permission");
        public static string RPDeleteError() => _DeleteError("Role Permission");
        public static string RPToggleError() => _ToggleError("Role Permission");
        #endregion

        #region User
        public static string UCreateSuccess() => _CreateSuccess("User");
        public static string UUpdateSuccess() => _UpdateSuccess("User");
        public static string UDeleteSuccess() => _DeleteSuccess("User");
        public static string UToggleSuccess() => _ToggleSuccess("User");
        public static string UCreateError() => _CreateError("User");
        public static string UUpdateError() => _UpdateError("User");
        public static string UDeleteError() => _DeleteError("User");
        public static string UToggleError() => _ToggleError("User");
        public static string UResetPasswordSuccess() => "User Password Reset Successfully.";
        public static string UResetPasscodeSuccess() => "User Passcode Reset Successfully.";
        public static string UResetPasswordError() => "Error Resetting User Password.";
        public static string UResetPasscodeError() => "Error Resetting User Passcode.";
        #endregion

    }
}
