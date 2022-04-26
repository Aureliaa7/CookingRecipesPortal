const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
    ],
    target: "https://localhost:7020",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
