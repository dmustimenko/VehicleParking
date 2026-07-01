using VehicleParking.Domain.Exceptions;
using VehicleParking.Domain.Helpers;

namespace VehicleParking.Domain.Values;

public sealed record VehicleReg
{
    private const int MaxLength = 20;
    
    private VehicleReg(string value) => Value = value;

    public string Value { get; }

    public static VehicleReg VerifyAndCreate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidVehicleRegException(value);
        }

        string normalizedValue = VehicleRegHelpers.Normalize(value);
        return normalizedValue.Length is 0 or > MaxLength
            ? throw new InvalidVehicleRegException(value)
            : new VehicleReg(normalizedValue);
    }
}
