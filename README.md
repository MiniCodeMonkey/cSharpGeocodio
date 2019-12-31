# A wrapper to access Geocodio, a geocoding service.  

## Website for the service: https://geocod.io/

# Please note, this is meant for accesing the Geocodio version 1.2 API.  For higher versions, please use the main branch or an applicable branch (if it exists).

### To use, install the nuget package cSharpGeocodio version along track version 1.0.* (currently we are at 1.0.3)

Usage and examples are below.

Note on exceptions and errors from Geocodio:

Wrap all calls in a try/catch for an AggregateException.  The inner exception is a GeocodingException with a property GeocodioErrorMessage which contains info on the error returned by Geocodio:

```C#
try {
    //Sample using ForwardGeocodeAsync, same for everseGeocodeAsync
    geocoder.ForwardGeocodeAsync(...)
    }
catch (AggregateException ex) {
    GeocodingException gex = (GeocodingException)ex.InnerException;
    gex.GeocodioErrorMessage;
    }

```

Forward Gecoding:

```c#
GeoCoderV2 geoCoder = new GeoCoderV2('Your Grocodio API key.  You get 2500 free lookups per day!');

//
//Forward geocode a single address:
//
string singleAddress = "2100 East Market Street, Philadelphia, PA 19103";

Task<BatchForwardGeoCodeResult> singleAddress = await geoCoder.ForwardGeocodeAsync(singleAddress
, QueryCongressional.No
, QueryStateLegislature.No
, QuerySchoolDistrict.No
, QueryCensusInfo.No
, QueryTimeZone.No);
                         
BatchFowardGeocodeResult singleResult = singleAddress.Result;

//Geocodio will often return multiple items in the Results 
//property of the Response object, ordered by the most accurate.
Location singleLatLong = single_result.Response.Results[0].Location;

//
//Batch forward geocoding
//
//Make a list of addresses...
Task<BatchForwardGeoCodeResult> batchGeocode = await geoCoder.ForwardGeocodeAsync(list_of_addresses
, QueryCongressional.No
, QueryStateLegislature.No
, QuerySchoolDistrict.No
, QueryCensusInfo.No
, QueryTimeZone.No);
                         
BatchForwardGeocodeResult batchResults = batchGeocode.Result;
//Iterate through collection of results.
//When batch geocoding, Geocodio returns the results in the same
//order as found in the list_of_addresses you pass to the ForwardGeocodeAsync method.
for(int i = 0; i++; i < batchResults.Results)
{
    BatchForwardGeoCodeRecord geoCodedItem = batchResults.Results[i];
    string addressWhichWasGeocoded = geoCodedItem.Query;
    Location latLong = geoCodedItem.Response.Results[0].Location;
    //Add to database, add to queue, some other operation;
}
```

Reverse Geocoding:

```c#
//
//Reverse geocode a single point
//

string singlePoint = "39.9373426,-75.1865927";

Task<BatchReverseGeoCodingResult> reversePoint = await gc.ReverseGeocodeAsync(singlePoint
, QueryCongressional.No
, QueryStateLegislature.No
, QuerySchoolDistrict.Yes
, QueryCensusInfo.No
, QueryTimeZone.No);

BatchReverseGeocodingResult singlePointResult = reversePoint.Result;
ReverseGeocodeResult reverseGeocodeResult = singlePointResult.Results[0].Response.Results;
string addressOfPoint = reverseGeocodeResult.Results[0].FormattedAddress;

//
//Batch reverse geocoding
//

List<string> batchReverseInputs = new List<string>();
//Add points to list...
Task<BatchReverseGeoCodingResult> batchReverse = await gc.ReverseGeocodeAsync(batchReverseInputs
, QueryCongressional.No
, QueryStateLegislature.No
, QuerySchoolDistrict.Yes
, QueryCensusInfo.No
, QueryTimeZone.No);

BatchReverseGeoCodingResult batchReverseResults = batchReverse.Result;
//Iterate through the results
//As with forward geocoding, results are returned in the same order as found
//in the list of LatLong points we passed to the ReverseGeocodeAsync
for (int i = 0; i < batchReverseResults.Length; i++)
{
    BatchReverseGeoCodeResponse batchResponse = batchReverseResults.Results[i];
    ReverseGeoCodeResult resultsFromOneQuery = batchResponse.Response;
    //Here, the actual results of the operation, ordered in terms
    //of accuray
    GeoCodeInfo[] geoCodedInfo = resultsFromOneQuery.Results;
}

```