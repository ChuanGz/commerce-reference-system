import matplotlib.pyplot as plt
import matplotlib.dates as mdates
from datetime import datetime, timedelta

# Define milestones and durations
milestones = [
    ("Project Setup and Architecture", 14),
    ("Identity Service and Authentication", 14),
    ("Backend Core Services", 21),
    ("Frontend Integration", 21),
    ("Payment Integration and Testing", 14),
    ("Deployment and Monitoring", 14),
    ("Final Review and Handover", 7),
]

# Define start date
start_date = datetime(2024, 12, 6)

# Calculate start and end dates for each milestone
dates = []
current_start = start_date
for name, duration in milestones:
    current_end = current_start + timedelta(days=duration)
    dates.append((name, current_start, current_end))
    current_start = current_end  # Next start is this end

# Plot Gantt chart
fig, ax = plt.subplots(figsize=(12, 6))

# Create Gantt bars
for i, (name, start, end) in enumerate(dates):
    ax.barh(i, (end - start).days, left=start, height=0.5, color="skyblue", edgecolor="black")
    ax.text(start + (end - start) / 2, i, name, ha="center", va="center", fontsize=10, color="black")

# Format the chart
ax.set_yticks(range(len(dates)))
ax.set_yticklabels([name for name, _, _ in dates])
ax.set_xlabel("Timeline")
ax.set_title("Gantt Chart for Project Milestones")
ax.xaxis.set_major_locator(mdates.MonthLocator())
ax.xaxis.set_major_formatter(mdates.DateFormatter("%b %d"))
plt.xticks(rotation=45)
plt.tight_layout()

# Show the Gantt chart
plt.show()
