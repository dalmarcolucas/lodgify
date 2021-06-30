using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PreparationTimeTests
    {
        private readonly HttpClient _client;

        public PreparationTimeTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        public class PrepationTestDataNotAllow : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2021, 7, 17), 1, false };
                yield return new object[] { new DateTime(2021, 7, 15), 1, false };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class PrepationTestDataAllow : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2021, 7, 19), 1, true };
                yield return new object[] { new DateTime(2021, 7, 20), 1, true };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(PrepationTestDataNotAllow))]
        public async Task GivenPrepartionTime_ShouldNotAllowNewBookingInBlockedDates(DateTime start, int nights, bool allowBooking)
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2021, 7, 15)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = nights,
                Start = start
            };
            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
                {
                }
            });
        }

        [Theory]
        [ClassData(typeof(PrepationTestDataAllow))]
        public async Task GivenPrepartionTime_ShouldAllowNewBookingInBlockedDates(DateTime start, int nights, bool allowBooking)
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2021, 7, 15)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = nights,
                Start = start
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

        }
    }
}
