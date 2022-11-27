using System.Net;

namespace KingMarvel.Application.Exceptions
{
    public class KingMarvelException : Exception
    {
        #region properties

        public HttpStatusCode HttpStatusCode { get; }

        public object CustomData { get; }

        #endregion

        #region constructors

        public KingMarvelException(string message = "", HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, object customData = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            CustomData = customData;
        }

        #endregion
    }
}
