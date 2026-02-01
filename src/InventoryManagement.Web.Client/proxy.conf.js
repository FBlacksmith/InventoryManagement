const { env } = require('process');

const target = env['services__web__https__0'] || env['services__web__http__0'] || 'https://localhost:57679';

console.log('Proxy target:', target);

const PROXY_CONFIG = {
    '/api': {
        target: target,
        secure: false,
        changeOrigin: true
    }
};

module.exports = PROXY_CONFIG;
