const https = require('https');
const express = require('express');
const forge = require('node-forge');

(function main() {
  const server = https.createServer(
    generateX509Certificate([
      { type: 6, value: 'http://localhost' },
      { type: 7, ip: '127.0.0.1' }
    ]), 
    makeExpressApp()
  );
  server.listen(8080, () => {
    console.log('Listening on https://localhost:8080/');
  });
})();

function generateX509Certificate(altNames) {
  const issuer = [
    { name: 'commonName', value: 'example.com' },
    { name: 'organizationName', value: 'E Corp' },
    { name: 'organizationalUnitName', value: 'Washington Township Plant' }
  ];
  const certificateExtensions = [
    { name: 'basicConstraints', cA: true },
    { name: 'keyUsage', keyCertSign: true, digitalSignature: true, nonRepudiation: true, keyEncipherment: true, dataEncipherment: true },
    { name: 'extKeyUsage', serverAuth: true, clientAuth: true, codeSigning: true, emailProtection: true, timeStamping: true },
    { name: 'nsCertType', client: true, server: true, email: true, objsign: true, sslCA: true, emailCA: true, objCA: true },
    { name: 'subjectAltName', altNames },
    { name: 'subjectKeyIdentifier' }
  ];
  const keys = forge.pki.rsa.generateKeyPair(2048);
  const cert = forge.pki.createCertificate();
  cert.validity.notBefore = new Date();
  cert.validity.notAfter = new Date();
  cert.validity.notAfter.setFullYear(cert.validity.notBefore.getFullYear() + 1);
  cert.publicKey = keys.publicKey;
  cert.setSubject(issuer);
  cert.setIssuer(issuer);
  cert.setExtensions(certificateExtensions);
  cert.sign(keys.privateKey);
  return {
    key: forge.pki.privateKeyToPem(keys.privateKey),
    cert: forge.pki.certificateToPem(cert)
  };
}

function makeExpressApp() {
  const app = express();
  const expressStaticGzip = require('express-static-gzip');
  app.use('/', expressStaticGzip(__dirname + '/Build', {
    enableBrotli: true,
    orderPreference: ['br', 'gzip'],
    setHeaders: (res, path) => {
      if (path.endsWith('.br')) {
        res.setHeader('Content-Encoding', 'br');
        res.setHeader('Content-Type', 'application/wasm');
      } else if (path.endsWith('.gz')) {
        res.setHeader('Content-Encoding', 'gzip');
        res.setHeader('Content-Type', 'application/wasm');
      }
    }
  }));
  return app;
}
