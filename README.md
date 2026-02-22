# TapFlow.API


GET /health must check database connectivity and return:
```
{
  "status": "ok",
  "db": "ok"
}
```
POST /v1/pours request example:
```
{
  "eventId": "3f1f2b3c-8b1a-4c2b-9c23-9f2d2e8a1b11",
  "deviceId": "tap-001",
  "locationId": "istanbul-kadikoy-01",
  "productId": "guinness",
  "startedAt": "2026-02-18T10:15:00Z",
  "endedAt": "2026-02-18T10:15:06Z",
  "volumeMl": 568
}
```

GET /v1/taps/{deviceId}/summary?from=...&to=... response example:
```
{
  "deviceId": "tap-001",
  "from": "2026-02-18T00:00:00Z",
  "to": "2026-02-19T00:00:00Z",
  "totalVolumeMl": 1325,
  "totalPours": 3,
  "topProduct": {
    "productId": "guinness",
    "volumeMl": 852,
    "pours": 2
  },
  "topLocation": {
    "locationId": "istanbul-kadikoy-01",
    "volumeMl": 1325,
    "pours": 3
  },
  "byProduct": [
    {
      "productId": "guinness",
      "volumeMl": 852,
      "pours": 2
    },
    {
      "productId": "ipa",
      "volumeMl": 473,
      "pours": 1
    }
  ],
  "byLocation": [
    {
      "locationId": "istanbul-kadikoy-01",
      "volumeMl": 1325,
      "pours": 3
    }
  ]
}
```
## Step 2: Validation Rules
Use these predefined products and locations:
```
{
  "productIds": [
    "guinness",
    "ipa",
    "lager",
    "pilsner",
    "stout",
    "efes-pilsen",
    "efes-malt",
    "bomonti-filtresiz",
    "tuborg-gold",
    "tuborg-amber"
  ],
  "locationIds": [
    "istanbul-kadikoy-01",
    "istanbul-besiktas-01",
    "izmir-alsancak-01",
    "ankara-cankaya-01",
    "london-soho-01"
  ],
  "volumeMlAllowed": [
    200,
    250,
    284,
    330,
    355,
    400,
    473,
    500,
    568,
    1000
  ]
}
```