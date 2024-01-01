export interface blogListVm {
    blogs: blogVmForList[];
}

export interface detailedBlogVm {
    id: string;
    title: string;
    creationDate: Date;
    details: string | null;
    editDate: Date | null;
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