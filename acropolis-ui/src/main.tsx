import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { Provider } from 'react-redux'
import { store } from './store/store.ts'
import { ConfigProvider, theme } from 'antd'
import { BrowserRouter, useRoutes } from 'react-router'
import App from './App.tsx'
import { routes } from './routes.tsx'

const AppRoutes = () => useRoutes(routes);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ConfigProvider theme={{
      algorithm: theme.darkAlgorithm,
      token: {
        colorBgBase: "#1e293b",
        colorTextBase: "#e5e7eb",
        colorBgContainer: "#102240",
        colorPrimary: "#3489ff",
        colorInfo: "#3489ff"
      }
    }}>
      <Provider store={store}>
        <BrowserRouter>
          <AppRoutes />
        </BrowserRouter>
      </Provider>
    </ConfigProvider>
  </StrictMode>,
)
