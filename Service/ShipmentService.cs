using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ShipmentApi.Services
{
    public class ShipmentService
    {
        private readonly IConfiguration _configuration;

        public ShipmentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ShipmentStatus>> GetShipmentStatuses(DateTime startDate, DateTime endDate)
        {
            var shipmentStatuses = new List<ShipmentStatus>();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetLatestShipmentStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            shipmentStatuses.Add(new ShipmentStatus
                            {
                                TrackingNumber = reader.GetString(0),
                                ShipmentDate = reader.GetDateTime(1),
                                LatestShipmentStatus = reader.GetString(2),
                                StatusDate = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            return shipmentStatuses;
        }
    }
}
