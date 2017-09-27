namespace DAX.Cson.Converters
{
    /*

   <PositionPoint>
    <sequenceNumber>1</sequenceNumber>
    <xPosition>549098.984682498</xPosition>
    <yPosition>6188454.03898898</yPosition>
    <Location ref="8eb271be-abfd-48cb-a222-b541bc2b8d83" />
  </PositionPoint>
  <PositionPoint>
    <sequenceNumber>1</sequenceNumber>
    <xPosition>549098.944771665</xPosition>
    <yPosition>6188454.03898898</yPosition>
    <Location ref="aaeb7b5c-2674-466a-ba22-639668109c8a" />
  </PositionPoint>
  <PositionPoint>
    <sequenceNumber>2</sequenceNumber>
    <xPosition>549098.944771665</xPosition>
    <yPosition>6188453.93900185</yPosition>
    <Location ref="aaeb7b5c-2674-466a-ba22-639668109c8a" />
  </PositionPoint>
 
    */
    //class PositionPointConverter : JsonConverter
    //{
    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        var positionPoint = (PositionPoint)value;

    //        if (positionPoint == null)
    //        {
    //            writer.WriteNull();
    //            return;
    //        }

    //        var location = positionPoint.Location;
    //        var refValue = new RefValue(location.referenceType, location.@ref);
    //        var refString = refValue.Serialize();

    //        var stringToWrite = $"{positionPoint.sequenceNumber}: {refString} ({positionPoint.xPosition},{positionPoint.yPosition})";

    //        writer.WriteValue(stringToWrite);
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        var stringValue = reader.ReadAsString();
    //        var firstIndex = stringValue.IndexOf(':');
    //        var sequenceNumber = stringValue.Substring(0, firstIndex).Trim();

    //        var theRest = stringValue.Substring(firstIndex + 1).Trim();
    //        var parts = theRest.Split(' ');
    //        var refValue = RefValue.Parse(parts[0]);

    //        return new PositionPoint
    //        {
    //            sequenceNumber = sequenceNumber,
    //            Location = new PositionPointLocation
    //            {
    //                @ref = refValue.Ref,
    //                referenceType = refValue.ReferenceType,
    //            }
    //        };
    //    }

    //    public override bool CanConvert(Type objectType)
    //    {
    //        return objectType == typeof(PositionPoint);
    //    }
    //}
}