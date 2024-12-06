# Define milestones and durations (split into FE and BE tasks for parallel implementation)
detailed_milestones = [
    ("Project Setup (FE + BE)", "Day 1-3"),
    ("API Contracts Definition (Shared)", "Day 4-6"),
    ("Authentication (BE: Identity Service)", "Day 7-14"),
    ("Login/Registration (FE)", "Day 7-14"),
    ("User Management BE (BE: User Service)", "Day 15-22"),
    ("User Management UI (FE: Backoffice)", "Day 15-22"),
    ("Product Service Implementation (BE)", "Day 23-30"),
    ("Product Listing and Details (FE)", "Day 23-30"),
    ("Order Management BE (Order Service)", "Day 31-38"),
    ("Order Creation & History (FE)", "Day 31-38"),
    ("Inventory Service Implementation (BE)", "Day 39-46"),
    ("Product Availability UI (FE)", "Day 39-46"),
    ("Payment Integration BE (Payment Service)", "Day 47-54"),
    ("Payment Workflow (FE)", "Day 47-54"),
    ("Integration & Testing (Shared)", "Day 55-60"),
]

# Prepare detailed Gantt chart
fig, ax = plt.subplots(figsize=(15, 8))

# Define y positions and labels for tasks
y_positions = range(len(detailed_milestones))
labels = [milestone[0] for milestone in detailed_milestones]
durations = [milestone[1] for milestone in detailed_milestones]

# Assign colors for FE, BE, and Shared tasks
colors = ["lightcoral" if "FE" in milestone[0] else "skyblue" if "BE" in milestone[0] else "lightgreen"
          for milestone in detailed_milestones]

# Plot bars for milestones
for i, (label, duration, color) in zip(y_positions, detailed_milestones, colors):
    start_day, end_day = map(int, duration.split("Day ")[1].split("-"))
    ax.barh(i, end_day - start_day, left=start_day, height=0.8, color=color, edgecolor="black")
    ax.text(start_day + (end_day - start_day) / 2, i, label, ha="center", va="center", fontsize=9, color="black")

# Set up axes
ax.set_yticks(y_positions)
ax.set_yticklabels(labels)
ax.set_xlabel("Timeline (Days)")
ax.set_title("Detailed Gantt Chart for Parallel FE and BE Implementation")
ax.set_xticks(range(0, 61, 5))
ax.grid(axis="x", linestyle="--", alpha=0.5)

# Legend
legend_labels = {"lightcoral": "Frontend (FE)", "skyblue": "Backend (BE)", "lightgreen": "Shared"}
legend_handles = [plt.Rectangle((0, 0), 1, 1, color=color, edgecolor="black") for color in legend_labels.keys()]
ax.legend(legend_handles, legend_labels.values(), loc="upper right")

plt.tight_layout()
plt.show()
