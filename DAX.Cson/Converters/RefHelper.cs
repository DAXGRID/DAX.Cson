namespace DAX.Cson.Converters
{
    class RefValue
    {
        public string ReferenceType { get; }
        public string Ref { get; }

        public RefValue(string referenceType, string @ref)
        {
            ReferenceType = referenceType;
            Ref = @ref;
        }

        public string Serialize()
        {
            if (ReferenceType == null) return Ref;

            return $"{Ref}[{ReferenceType}]";
        }

        public static RefValue Parse(string refString)
        {
            var index = refString.IndexOf('[');

            if (index == -1) return new RefValue(null, refString);

            var firstPart = refString.Substring(0, index);
            var secondPart = refString.Substring(index + 1, refString.Length - 2);

            return new RefValue(secondPart, firstPart);
        }
    }
}