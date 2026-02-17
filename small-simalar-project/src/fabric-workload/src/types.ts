export interface ProductSummary {
  id: number;
  name: string;
  category: string;
  price: number;
}

export interface ProductDetail extends ProductSummary {
  description: string;
  stock: number;
  tags: string[];
}

export interface UserSummary {
  id: number;
  name: string;
  email: string;
  role: string;
}

export interface OrderSummary {
  id: number;
  userId: number;
  totalAmount: number;
  status: string;
  date: string;
}

export interface DashboardData {
  productCount: number;
  userCount: number;
  orderCount: number;
  totalRevenue: number;
  pendingOrders: number;
  deliveredOrders: number;
  cancelledOrders: number;
}
