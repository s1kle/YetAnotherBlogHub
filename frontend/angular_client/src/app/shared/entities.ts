export interface blogListVm {
    blogs: blogVmForList[];
}

export interface detailedBlogVm {
    id: string;
    title: string;
    creationDate: string;
    details: string | null;
    editDate: string | null;
}

export interface blogVmForList {
    id: string;
    title: string;
}

export interface createBlogVm {
    title: string;
    details: string | null;
}

export interface updateBlogVm {
    title: string;
    details: string | null;
}