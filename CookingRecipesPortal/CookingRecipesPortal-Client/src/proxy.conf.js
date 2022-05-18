const PROXY_CONFIG = [
  {
    context: [
      "/api/accounts",
      "/api/recipes",
    ],
    target: "https://localhost:7020",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
