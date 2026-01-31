/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: 'var(--sys-primary)',
        secondary: 'var(--sys-secondary)',
        tertiary: 'var(--sys-tertiary)',
        error: 'var(--sys-error)',
      }
    },
  },
  plugins: [],
}
