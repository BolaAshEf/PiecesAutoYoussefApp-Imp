using SharedORMAppsModels.DB.Base;
using System.Text.RegularExpressions;

namespace PiecesAutoYoussefApp.NotificationCenter
{
    public static class ErrorHandler
    {
        public static readonly string GeneralError = "An error has occurred!";
        public static readonly string PleaseChooseFirstError = "Please select items from the grid to delete!";
        public static readonly string AppAlreadyOpened = "The application is already running.";

        private static readonly Dictionary<string, string> _customMsgsErrorMap = new Dictionary<string, string>
        {
            { "err_general", GeneralError },
            { "err_invalidPieceProvided", "Invalid piece provided." },
            { "err_insufficientProductStock", "Insufficient product stock." },
        };

        private static string? ExtractConstraintErrorKey(string msg)
        {
            string regexPattern = "(?:'|\")((?:PK|UQ|CK)__[a-zA-Z_]*?)(?:'|\")";
            Regex regex = new Regex(regexPattern);
            Match match = regex.Match(msg);

            if (match.Success && match.Groups.Count == 2)
            {
                string key = match.Groups[1].Value;
                bool res1 = _constraintNameToEnumKeyMap.TryGetValue(key, out var constraintEnumKey);
                if (!res1) { return null; }
                bool res2 = _constraintEnumKeyToErrMsgMap.TryGetValue(constraintEnumKey, out var errMsg);
                if (!res2) { return null; }
                return errMsg;
            }
            else
            {
                return null;
            }
        }

        public static void NotifyErrorRes(DBErrorResponse error)
        {
            string msg;
            if (error.CustomMessage != null)
            {
                if (_customMsgsErrorMap.TryGetValue(error.CustomMessage, out var customMsg))
                {
                    msg = customMsg;
                }
                else
                {
                    msg = error.CustomMessage;
                }
            }
            else if (error.Message != null)
            {
                string? constraintErrorMsg = ExtractConstraintErrorKey(error.Message);
                if (constraintErrorMsg != null)
                {
                    msg = constraintErrorMsg;
                }
                else
                {
                    msg = GeneralError;
                }
            }
            else
            {
                msg = GeneralError;
            }

            NotifyError(msg);
        }

        public static void NotifyNullObjRes(params DBResponse[] responses)
        {
            foreach (DBResponse res in responses)
            {
                if (res is DBErrorResponse errRes)
                {
                    NotifyErrorRes(errRes);
                    return;
                }
            }

            NotifyErrorUnknown();
        }

        public static void NotifyErrorNullObj()
        {
            string msg = GeneralError;

            NotifyError(msg);
        }

        public static void NotifyErrorUnknown()
        {
            string msg = GeneralError;

            NotifyError(msg);
        }

        public static void NotifyErrorChooseFirst()
        {
            string msg = PleaseChooseFirstError;

            NotifyError(msg);
        }

        public static void NotifyAppAlreadyOpened()
        {
            string msg = AppAlreadyOpened;

            NotifyError(msg);
        }

        public static void NotifyError(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static Exception NotifyError(ConstraintKey key)
        {
            string msg = _constraintEnumKeyToErrMsgMap.TryGetValue(key, out var errMsg) ? errMsg : GeneralError;

            NotifyError(msg);

            return new Exception(msg);
        }


        private static readonly Dictionary<string, ConstraintKey> _constraintNameToEnumKeyMap = new Dictionary<string, ConstraintKey>{
            { "CK__tblClientCategories__categoryName__notEmpty", ConstraintKey.ClientCategoryNameNotEmpty },
            { "UQ__tblClientCategories__categoryName", ConstraintKey.ClientCategoryNameUnique },

            { "CK__tblClients__firstName__notEmpty", ConstraintKey.ClientFirstNameNotEmpty },
            { "CK__tblClients__lastName__notEmpty", ConstraintKey.ClientLastNameNotEmpty },
            { "CK__tblClients__phoneNumber__notEmpty", ConstraintKey.ClientPhoneNumberNotEmpty },
            { "CK__tblClients__clientAddress__notEmpty", ConstraintKey.ClientAddressNotEmpty },
            { "UQ__tblClients__phoneNumber", ConstraintKey.ClientPhoneNumberUnique },
            { "UQ__tblClients__firstName__lastName", ConstraintKey.ClientFullNameUnique },

            { "CK__tblModels__modelName__notEmpty", ConstraintKey.ModelNameNotEmpty },
            { "UQ__tblModels__modelName", ConstraintKey.ModelNameUnique },

            { "CK__tblPieces__pieceName__notEmpty", ConstraintKey.PieceNameNotEmpty },
            { "UQ__tblPieces__pieceName", ConstraintKey.PieceNameUnique },

            { "CK__tblProducts__refrenceID__notEmpty", ConstraintKey.ProductReferenceIDNotEmpty },
            { "UQ__tblProducts__refrenceID", ConstraintKey.ProductReferenceIDUnique },
            { "CK__tblProducts__stock__nonNegative", ConstraintKey.ProductStockNonNegative },
            { "CK__tblProducts__unitPrice__nonNegative", ConstraintKey.ProductUnitPriceNonNegative },
            { "UQ__tblProducts__pieceID__modelID", ConstraintKey.ProductPieceModelCombinationUnique },

            { "CK__tblSuppliers__productQuantity__nonNegative", ConstraintKey.SupplierProductQuantityNonNegative },
            { "CK__tblSuppliers__firstName__notEmpty", ConstraintKey.SupplierFirstNameNotEmpty },
            { "CK__tblSuppliers__lastName__notEmpty", ConstraintKey.SupplierLastNameNotEmpty },
            { "CK__tblSuppliers__phoneNumber__notEmpty", ConstraintKey.SupplierPhoneNumberNotEmpty },
            { "CK__tblSuppliers__supplierAddress__notEmpty", ConstraintKey.SupplierAddressNotEmpty },
            { "UQ__tblSuppliers__firstName__lastName", ConstraintKey.SupplierFullNameUnique },
            { "UQ__tblSuppliers__phoneNumber", ConstraintKey.SupplierPhoneNumberUnique },

            { "CK__tblOrders__quantity__positive", ConstraintKey.OrderQuantityPositive },
            { "CK__tblOrders__unitPrice__nonNegative", ConstraintKey.OrderUnitPriceNonNegative },
            { "CK__tblOrders__vatRate__nonNegative", ConstraintKey.OrderVatRateNonNegative }
        };

        private static readonly Dictionary<ConstraintKey, string> _constraintEnumKeyToErrMsgMap = new Dictionary<ConstraintKey, string>{
            { ConstraintKey.ClientCategoryNameNotEmpty, "The category name cannot be empty." },
            { ConstraintKey.ClientCategoryNameUnique, "The category name must be unique." },

            { ConstraintKey.ClientFirstNameNotEmpty, "The first name cannot be empty." },
            { ConstraintKey.ClientLastNameNotEmpty, "The last name cannot be empty." },
            { ConstraintKey.ClientPhoneNumberNotEmpty, "The phone number cannot be empty." },
            { ConstraintKey.ClientAddressNotEmpty, "The client address cannot be empty." },
            { ConstraintKey.ClientPhoneNumberUnique, "The phone number must be unique." },
            { ConstraintKey.ClientFullNameUnique, "The combination of first and last name must be unique." },

            { ConstraintKey.ModelNameNotEmpty, "The model name cannot be empty." },
            { ConstraintKey.ModelNameUnique, "The model name must be unique." },

            { ConstraintKey.PieceNameNotEmpty, "The piece name cannot be empty." },
            { ConstraintKey.PieceNameUnique, "The piece name must be unique." },

            { ConstraintKey.ProductReferenceIDNotEmpty, "The reference ID cannot be empty." },
            { ConstraintKey.ProductReferenceIDUnique, "The reference ID must be unique." },
            { ConstraintKey.ProductStockNonNegative, "The stock cannot be negative." },
            { ConstraintKey.ProductUnitPriceNonNegative, "The unit price cannot be negative." },
            { ConstraintKey.ProductPieceModelCombinationUnique, "The combination of piece ID and model ID must be unique." },

            { ConstraintKey.SupplierProductQuantityNonNegative, "The product quantity cannot be negative." },
            { ConstraintKey.SupplierFirstNameNotEmpty, "The supplier's first name cannot be empty." },
            { ConstraintKey.SupplierLastNameNotEmpty, "The supplier's last name cannot be empty." },
            { ConstraintKey.SupplierPhoneNumberNotEmpty, "The supplier's phone number cannot be empty." },
            { ConstraintKey.SupplierAddressNotEmpty, "The supplier's address cannot be empty." },
            { ConstraintKey.SupplierFullNameUnique, "The combination of the supplier's first and last name must be unique." },
            { ConstraintKey.SupplierPhoneNumberUnique, "The supplier's phone number must be unique." },

            { ConstraintKey.OrderQuantityPositive, "The quantity must be positive." },
            { ConstraintKey.OrderUnitPriceNonNegative, "The unit price cannot be negative." },
            { ConstraintKey.OrderVatRateNonNegative, "The VAT rate cannot be negative." }
        };
    }

    public enum ConstraintKey
    {
        ClientCategoryNameNotEmpty,
        ClientCategoryNameUnique,

        ClientFirstNameNotEmpty,
        ClientLastNameNotEmpty,
        ClientPhoneNumberNotEmpty,
        ClientAddressNotEmpty,
        ClientPhoneNumberUnique,
        ClientFullNameUnique,

        ModelNameNotEmpty,
        ModelNameUnique,

        PieceNameNotEmpty,
        PieceNameUnique,

        ProductReferenceIDNotEmpty,
        ProductReferenceIDUnique,
        ProductStockNonNegative,
        ProductUnitPriceNonNegative,
        ProductPieceModelCombinationUnique,

        SupplierProductQuantityNonNegative,
        SupplierFirstNameNotEmpty,
        SupplierLastNameNotEmpty,
        SupplierPhoneNumberNotEmpty,
        SupplierAddressNotEmpty,
        SupplierFullNameUnique,
        SupplierPhoneNumberUnique,

        OrderQuantityPositive,
        OrderUnitPriceNonNegative,
        OrderVatRateNonNegative,
    }

    public static class ConstraintKeyExtension
    {
        public static Exception NotifyErr(this ConstraintKey key) => ErrorHandler.NotifyError(key);
    }
}
