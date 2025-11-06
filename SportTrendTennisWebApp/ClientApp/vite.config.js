import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Polyfill `crypto.getRandomValues` for environments where it's missing (older Node.js)
// Vite (and some dependencies) expect the Web Crypto API's getRandomValues.
// This provides a minimal shim using Node's crypto.randomFillSync when needed.
if (typeof globalThis.crypto === 'undefined' || typeof globalThis.crypto.getRandomValues !== 'function') {
  try {
    // prefer webcrypto when available
    // @ts-ignore
    const nodeCrypto = await import('crypto');
    const webcrypto = nodeCrypto.webcrypto;
    if (webcrypto && typeof webcrypto.getRandomValues === 'function') {
      globalThis.crypto = webcrypto;
    } else {
      // fallback shim using randomFillSync
      const { randomFillSync } = nodeCrypto;
      globalThis.crypto = {
        getRandomValues: (arr) => {
          return randomFillSync(arr);
        }
      };
    }
  } catch (e) {
    // ignore - in very old Node versions this may fail; recommend upgrading Node instead
    // Vite will still error but user will see guidance to upgrade Node.
  }
}

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
})
