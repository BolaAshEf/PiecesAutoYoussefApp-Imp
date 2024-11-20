using PiecesAutoYoussefApp.NotificationCenter;
using SharedORMAppsModels.DB.Base;
using SharedORMAppsModels.Extensions;

namespace PiecesAutoYoussefApp.Extensions
{
    public static class PresentersExtensions
    {
        private static readonly DBResponse _ignoreRes = new DBErrorResponse();

        [Flags]
        public enum PresenterNotificationOption
        {
            Success = 1,
            Failure = 2,
            Always = Success | Failure,
        }

        private static IDBInvokable<RET> AlsoNotify<RET>(
            this IDBInvokable<RET> proc,
            Action<int>? notifyCallback = null,
            PresenterNotificationOption options = PresenterNotificationOption.Always) => new DBInvokableResHandlerDecorator<RET>(proc,
            resCallback =>
            {
                var res = resCallback();
                RET? obj = res.GetObjIfExist<RET>();
                if ((options & PresenterNotificationOption.Failure) == PresenterNotificationOption.Failure && res != _ignoreRes && obj == null)
                {
                    ErrorHandler.NotifyNullObjRes(res);
                }
                else if (notifyCallback != null && (options & PresenterNotificationOption.Success) == PresenterNotificationOption.Success && obj != null)
                {
                    notifyCallback.Invoke(res.RowsAffected ?? 0);
                }

                return res;
            });

        public static IDBInvokable<RET> AlsoNotifyFailure<RET>(this IDBInvokable<RET> proc) =>
            proc.AlsoNotify(null, PresenterNotificationOption.Failure);

        public static IDBInvokable<RET> AlsoNotifyAdd<RET>(
            this IDBInvokable<RET> proc,
            PresenterNotificationOption options = PresenterNotificationOption.Always) =>
            proc.AlsoNotify(i => InfoHandler.NotifyAddSave(i), options);

        public static IDBInvokable<RET> AlsoNotifyUpdate<RET>(
            this IDBInvokable<RET> proc,
            PresenterNotificationOption options = PresenterNotificationOption.Always) =>
            proc.AlsoNotify(i => InfoHandler.NotifyUpdateSave(i), options);

        public static IDBInvokable<RET> AlsoNotifyDelete<RET>(
            this IDBInvokable<RET> proc,
            PresenterNotificationOption options = PresenterNotificationOption.Always) =>
            proc.AlsoNotify(i => InfoHandler.NotifyDelete(i), options);

        public static IDBInvokable<RET> AlsoMakeSure<RET>(this IDBInvokable<RET> proc, Func<bool> callback) =>
            new DBInvokableResHandlerDecorator<RET>(
                proc,
                resCallback => callback() ? resCallback() : _ignoreRes
            );

        public static IDBInvokable<NoObj> AlsoAssureDeletion(this IDBInvokable<NoObj> proc, int count) =>
            proc
            .AlsoMakeSure(() => InfoHandler.AssureDeletion(count))
            .AlsoMakeSure(() =>
            {
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    ErrorHandler.NotifyErrorChooseFirst();
                    return false;
                }
            });


        public static IDBInvokable<RET> AlsoApply<RET>(
            this IDBInvokable<RET> proc,
            Func<RET?, RET?> applyCallback) => new DBInvokableResHandlerDecorator<RET>(
                proc,
                resCallback =>
                {
                    var res = resCallback();
                    if (res is DBObjectsResponse<RET> objRes)
                    {
                        res = new DBObjectsResponse<RET>()
                        {
                            RowsAffected = objRes.RowsAffected,
                            CustomMessage = objRes.CustomMessage,
                            Object = applyCallback(objRes.Object),
                        };
                    }

                    return res;
                }
            );
    }
}
