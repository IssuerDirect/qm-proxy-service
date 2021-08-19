const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');
const dotenv = require('dotenv');
const { qmAuth } = require('./middleware');
const cors = require('cors');

dotenv.config();

// Create Express Server
const app = express();

// Configuration
const PORT = process.env.PORT || 4730;
const QM_SERVICE_DOMAIN = process.env.QM_SERVICE_DOMAIN || 'http://app.quotemedia.com';

app.use(qmAuth);
app.use(cors());

// Proxy endpoints
app.use('/api/qm', createProxyMiddleware({
  target: QM_SERVICE_DOMAIN,
  changeOrigin: true,
  pathRewrite: {
    [`^/api/qm`]: '',
  },
}));

// Start the Proxy
app.listen(PORT, () => {
  console.log(`Starting Proxy on port ${PORT}`);
});
