export interface ResourceModel {
  id: string;
  title: string | null;
  description: string;
  url: string;
  source: string;
  viewed: boolean;
}

export interface Page extends ResourceModel {
  
}

export interface Videos extends ResourceModel {
  
}