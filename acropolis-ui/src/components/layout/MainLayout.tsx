import { LaptopOutlined, NotificationOutlined, UserOutlined } from "@ant-design/icons";
import { Breadcrumb, Layout, Menu, MenuProps, theme } from "antd";
import { Content, Header } from "antd/es/layout/layout";
import Sider from "antd/es/layout/Sider";
import { MenuItemType } from "antd/es/menu/interface";
import React, { useState } from "react";
import { FC } from "react"
import { Outlet } from "react-router";

const items1: MenuItemType[] = ['1', '2', '3'].map((key) => ({
  key,
  label: `nav ${key}`,
}));

const items2: MenuItemType[] = [UserOutlined, LaptopOutlined, NotificationOutlined].map(
  (icon, index) => {
    const key = String(index + 1);

    return {
      key: `sub${key}`,
      icon: React.createElement(icon),
      label: `subnav ${key}`,
      children: Array.from({ length: 4 }).map((_, j) => {
        const subKey = index * 4 + j + 1;
        return {
          key: subKey,
          label: `option${subKey}`,
        };
      }),
    };
  },
);

export const MainLayout: FC = () => {
  const [isSiderCollapsed, setIsSiderCollapsed] = useState<boolean>(false);

  return (
    <Layout className="h-full w-full">
      <Header className="flex p-0">
        <div className="demo-logo m-3 bg-slate-500 w-44 block rounded-md"></div>
        <Menu
          className="flex flex-1"
          theme="dark"
          mode="horizontal"
          defaultSelectedKeys={['1']}
          items={items1}
        />
      </Header>
      <Layout className="h-full w-full">
        <Sider width={200} collapsible collapsed={isSiderCollapsed} onCollapse={() => setIsSiderCollapsed(!isSiderCollapsed)}>
          <Menu
            mode="inline"
            defaultSelectedKeys={['1']}
            defaultOpenKeys={['sub1']}
            style={{ height: '100%', borderRight: 0 }}
            items={items2}
          />
        </Sider>
        <Layout>
          <Breadcrumb className="px-4"
            items={[{ title: 'Home' }, { title: 'List' }, { title: 'App' }]}
            style={{ margin: '16px 0' }}
          />
          <Content className="px-4">
            <Outlet />
          </Content>
        </Layout>
      </Layout>
    </Layout>
  );
}