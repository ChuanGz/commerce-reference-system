# 🧠 AI Frontend Application Guide  
*(Enterprise CRUD SPA – Internal AI Prompt)*

Tài liệu này được sử dụng như:
- **AI Prompt nội bộ** cho Copilot / ChatGPT / Cursor
- **Coding guide chuẩn** cho frontend team

Mục tiêu:
- Viết application frontend (không phải UI library)
- Reuse UI Core có sẵn
- Tập trung CRUD, API integration
- Code dễ đọc, dễ sửa, dễ onboard

---

# PHẦN 1 – IMPLEMENTATION

## 1. Phạm vi & tư duy chung

- Dự án là SPA thuần
- Không xây UI system
- Không thể hiện kiến trúc phức tạp không cần thiết
- Code phải dễ hiểu cho người mới và dễ sửa cho người sau

Luôn tự hỏi:
> “Nếu 6 tháng nữa người khác sửa file này thì có hiểu không?”

---

## 2. UI Core – Cách sử dụng đúng

- Chỉ sử dụng UI Core
- Không wrap, không fork, không copy logic UI Core
- Thiếu component → request UI Core team
- App không gánh trách nhiệm UI system

---

## 3. Data Flow – React Query & Zustand

### 3.1 Phân vai rõ ràng

**React Query**
- Quản lý server state
- Fetch data từ API
- Cache, refetch, invalidate

**Zustand**
- Quản lý client/UI state:
  - Filter
  - Pagination
  - Sorting
  - Selected rows
  - UI mode
  - Feature flag

Không trộn vai trò.
Không dùng Zustand làm nguồn data fetch chính.

---

### 3.2 Nguyên tắc dùng data từ React Query

- Dùng data trực tiếp từ React Query
- Sau mutation → invalidate hoặc refetch
- Tránh copy server data vào Zustand

Mục tiêu:
- Tránh stale data
- Tránh double source of truth
- Dễ maintain

---

### 3.3 CRUD & Refetch strategy

- Create → refetch list
- Update → refetch list / detail
- Delete → refetch list
- Reload page → data phải đúng

Ưu tiên độ đúng của data hơn tối ưu sớm.

---

## 4. DataGrid / Table

- Dùng DataGrid của UI Core
- Ưu tiên server-side pagination, sorting, filtering

**State management**
- React Query: data
- Zustand: filter, pagination, sorting, grouping

**Performance**
- Memo column definition
- Tránh inline function trong render
- Không re-render table không cần thiết

---

## 5. Form & Validation

### 5.1 Khai báo form

- Dùng useForm
- Type rõ ràng
- Chỉ khai báo field cần dùng
- Luôn có defaultValues

---

### 5.2 Validation

- Dùng Zod schema + resolver
- Schema là single source of truth
- Không validate thủ công rải rác

---

### 5.3 Behavior & performance

- isValid, isDirty realtime
- Không submit khi invalid
- Tránh watch toàn form
- Form lớn → tách component

---

## 6. Routing

- Dùng TanStack Router
- Không gọi router API trực tiếp khắp nơi
- Routing thông qua abstraction để tránh lock-in

**URL Query Param**
- Dùng cho filter, sort, pagination
- Không dùng cho data nhạy cảm
- Anything in URL = public

---

## 7. Feature Flag

- Bắt buộc support feature flag
- Feature flag là configuration, không phải business logic
- Bật/tắt dễ, test độc lập

---

# PHẦN 2 – TEST CHECKLIST TRƯỚC KHI PUSH PR

## Chức năng
- CRUD hoạt động đúng
- Update/Delete xong có refetch
- Reload page không stale data

## Data & State
- React Query dùng đúng vai trò
- Không copy server data vào Zustand

## Form
- Có defaultValues
- Validation đúng
- Không re-render thừa

## Table
- Pagination, filter, sort đúng
- Performance ổn với data lớn

## Routing
- Query param không leak thông tin nhạy cảm

## Clean code
- Không console.log
- Không alert
- Không debugger
- Code dễ đọc, không overengineering

---

# PHẦN 3 – PUSH PR & VIẾT DESCRIPTION

## Trước khi push
- Code chạy local OK
- Đã tự review checklist
- Không commit debug code

---

## PR scope
- PR nhỏ, rõ mục tiêu
- Không nhồi nhiều feature không liên quan

---

## PR description

**Mô tả ngắn gọn**
- PR này làm gì
- Giải quyết vấn đề gì

**Thay đổi chính**
- Màn hình / flow nào
- CRUD nào

**Lưu ý cho reviewer**
- Điểm cần chú ý
- Trade-off (nếu có)

Tone:
- Rõ ràng
- Không khoe kỹ thuật
- Viết để review nhanh

---

## Kết luận

Guide này là baseline, không phải luật cứng.

Có thể phá nếu có lý do kỹ thuật hợp lý.

Code sạch vì hiểu người, không vì rule.