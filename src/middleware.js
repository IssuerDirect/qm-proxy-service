
const QM_AUTH_TOKEN = process.env.QM_AUTH_TOKEN || 'NzllMWQxZDktODJhNi00MjY5LTllY2YtMzk1NWU5MWZmNDFk';
const QM_WEBMASTER_ID = process.env.QM_WEBMASTER_ID || '96484';

const qmAuth = (req, res, next) => {
  req.headers.authorization = `Bearer ${QM_AUTH_TOKEN}`;
  req.query = {
    ...req.query,
    webmasterId: QM_WEBMASTER_ID,
  };
  next();
};

module.exports = {
  qmAuth,
};
