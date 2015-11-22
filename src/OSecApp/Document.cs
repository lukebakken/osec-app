namespace OSecApp
{
    using System;

    public class Document
    {
        private readonly DocumentOptions opts;

        public Document(DocumentOptions opts)
        {
            this.opts = opts;
            if (this.opts == null)
            {
                throw new ArgumentNullException("opts", Properties.Resources.OptionsAreRequiredException);
            }
        }
    }
}
