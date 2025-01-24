const { defineConfig } = require('@vue/cli-service')
const path = require('path')

module.exports = defineConfig({
  transpileDependencies: true,
  outputDir: path.resolve(__dirname, '../wwwroot'),
  devServer: {
    proxy: {
      '/': {
        target: 'http://localhost:5115', 
        changeOrigin: true,
      },
    },
  },
})
