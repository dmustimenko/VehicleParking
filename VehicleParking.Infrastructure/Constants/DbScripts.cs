namespace VehicleParking.Infrastructure.Constants;

public static class DbScripts
{
    public const string AllocateParkingSpaceSql =
        """
            SELECT allocate_next_free_parking_space() AS "Value"
        """;

    public const string AllocateParkingSpaceFuncCreate =
        """
        CREATE OR REPLACE FUNCTION allocate_next_free_parking_space()
        RETURNS integer
        LANGUAGE sql
        AS $$
            SELECT ps."Number"
            FROM parking_spaces AS ps
            WHERE NOT EXISTS (
                SELECT 1
                FROM parking_sessions AS s
                WHERE s."SpaceNumber" = ps."Number"
                    AND s."TimeOut" IS NULL
            )
            ORDER BY ps."Number"
            LIMIT 1
            FOR UPDATE OF ps SKIP LOCKED;
        $$;
        """;

    public const string AllocateParkingSpaceFuncDrop =
        "DROP FUNCTION IF EXISTS allocate_next_free_parking_space();";
}