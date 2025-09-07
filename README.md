# Pricing Assessment

## Setup
1. Clone the repo
2. Configure connection string (SQL Server / LocalDB)
3. Run migrations: `dotnet ef database update`
4. Run the API
5. Run tests

## Assumptions
- Currency conversion rates are fixed (see RateProvider)
- Caching TTL = 30s

## Time Spent
~5 hours

## Trade-offs
- Simplified error handling for demo purposes
- Only one integration test for best-price, more could be added
