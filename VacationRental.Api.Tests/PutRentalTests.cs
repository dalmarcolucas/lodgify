using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAGetReturnsTheUpdatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 3
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            RentalViewModel getResult;
            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }

            var requestPut = new RentalBindingModel
            {
                Units = 12,
                PreparationTimeInDays = 6
            };

            ResourceIdViewModel putResult;
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{getResult.Id}", requestPut))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
                putResult = await putResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getPutResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(requestPut.Units, getPutResult.Units);
                Assert.Equal(requestPut.PreparationTimeInDays, getPutResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ShouldNotAllowWhenPutCausesAOverlap()
        {
            var request = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 3
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            RentalViewModel getResult;
            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = getResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = getResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 08)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

            var requestPut = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 6
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{getResult.Id}", requestPut))
                {
                }
            });

            var requestPut2 = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 6
            };

            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{getResult.Id}", requestPut2))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
            }
        }
    }
}
