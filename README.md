# ATM App Using C#

## Technical Specification

### Core Features

1. **User Authentication:**

   - Users can log in with a username and PIN.
   - Stores user data (username, PIN, balance, and transactions) in a JSON file.

2. **Account Operations:**

   - **Check Balance**: Allows users to view their account balance.
   - **Deposit Money**: Enables users to deposit money into their account.
   - **Withdraw Money**: Allows users to withdraw money, with balance validation.

3. **International Money Transfer:**

   - Send money to another account, even in a different country.
   - Utilizes a currency conversion API (e.g., ExchangeRate-API or Open Exchange Rates) for converting amounts to the recipient's currency.
   - Stores transaction details (sender, receiver, amount, currency, conversion rate, timestamp) in a JSON file.

4. **Transaction History:**

   - Users can view a list of all transactions (deposits, withdrawals, and transfers).

5. **Exit:**
   - Logs out and exits the application.

## Installation

1. Clone the repository:

   ```bash
   git clone <repository-url>
   ```

2. Open the project in your preferred C# IDE.

3. Ensure that the necessary API keys for the currency conversion API are set in the CurrencyConverter.cs file.

4. Build and run the application.

### API KEYS CAN BE CREATED HERE:

https://www.exchangerate-api.com/docs/overview
