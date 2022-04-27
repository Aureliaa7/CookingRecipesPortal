const PROXY_CONFIG = [
  {
    context: [
      "/api/accounts",
    ],
    target: "https://localhost:7020",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
