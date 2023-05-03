/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx,vue}",
  ],
  theme: {
    extend: {
      fontFamily: () => ({
        'sans': ['Inter', 'Open Sans', 'sans-serif'],
      })
    },
  },
  plugins: [],
}

